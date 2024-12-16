using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityHFSM;

public enum GridPhase {Placement, Battle}

[Serializable]
public struct GridVisual
{
    public Sprite TileSprite;
    public float TileScale;
}

[Serializable]
public struct GridPrep
{
    public int NumSpawnRows; // 2
    public int UnitCostTotal; // 20
    public int NumPlayers; // 2
    public int MovementPoints; // 2
    public int ActionPoints; // 3
}

public class GridManager : MonoBehaviour
{
    Position<Tile> _tiles;
    Position<Unit> _units;
    MovementHandler _movementHandler;
    InputHandler _inputHandlerDragDrop;
    PlayerManager _playerManager;
    StateMachine _stateMachine;

    GridPhase _gridPhase;
    Unit _unitDragging;
    int _turn;
    int _tileHovered;
    int _tileSelected;

    public int X;
    public int Y;
    public GridVisual Visual;
    public GridPrep Prep;
    public GameInfoDisplay GameInfoDisplay;
    public UnitInfoDisplay UnitInfoDisplay;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Singleton.TileHoverEvent += TileHover;
        EventManager.Singleton.UnitPlaceEvent += UnitPlace;
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        HandleUnitDrag();
        _stateMachine.OnLogic();
    }

    void Init()
    {
        _tiles ??= new Position<Tile>(X, Y);
        _units ??= new Position<Unit>(X, Y);
        _movementHandler ??= new MovementHandler();
        _inputHandlerDragDrop ??= new InputHandler(InputActionPreset.DragDrop);
        _playerManager = new PlayerManager(Prep.NumPlayers, Prep.MovementPoints, Prep.ActionPoints);
        InitStateMachine();

        _gridPhase = GridPhase.Placement;
        _unitDragging = null;
        _turn = 0;
        _tileHovered = 0;
        _tileSelected = 0;
        
        CreateTiles();
    }

    void InitStateMachine()
    {
        _stateMachine = new StateMachine();
        _stateMachine.AddState("Idle", onEnter => StateOnEnterIdle());
        _stateMachine.AddState("TileSelected", onEnter => StateOnEnterTileSelected());
        _stateMachine.AddState("MoveSelected", onEnter => StateOnEnterMoveSelected());
        _stateMachine.SetStartState("Idle");
        _stateMachine.AddTwoWayTransition("Idle", "TileSelected", transition => _tileSelected != 0);
        _stateMachine.AddTransition("MoveSelected", "Idle", transition => _tileSelected == 0);
        // _stateMachine.AddTransition("TileSelected", "ActionSelected");
        // _stateMachine.AddTransition("ActionSelected", "Idle");
        _stateMachine.Init();
    }

    void CreateTiles()
    {
        Tile tileGO = Util.CreateGameObject<Tile>();
        for (int j = 1; j <= Y; j++)
        {
            for (int i = 1; i <= X; i++)
            {
                Tile tile = Instantiate(tileGO, Util.Get2DWorldPos(new Vector3Int(i, j, 0), Visual.TileScale), Quaternion.identity, transform);
                tile.Init(Visual.TileSprite, Visual.TileScale, _tiles.GetIndex(new Vector2Int(i, j)));
                _tiles.AddValue(tile);
                _units.AddValue(null);
            }
        }
        Destroy(tileGO.gameObject);
    }

    void TileHover(int id)
    {
        _tileHovered = id;
    }

    void UnitPlace(Unit unit)
    {
        _unitDragging = null;
        EventManager.Singleton.StartUnitUIUpdateEvent(unit.Stats.PlayerController, unit.ListUIPosition, PlaceUnit(unit, _tileHovered));
    }

    public void OnSelect(InputAction.CallbackContext ctx) 
    {
        // On Mouse Press on Grid
        if (ctx.started && _tileHovered != 0)
        {
            if (_gridPhase == GridPhase.Placement)
            {
                HandleUnitPlacement();
            }
            else if (_gridPhase == GridPhase.Battle)
            {
                if (_stateMachine.ActiveStateName == "MoveSelected")
                {
                    HandleMoveSelect();
                }
                else
                {
                    HandleTileSelect();
                }
            }
        }
    }

    public void StartGame()
    {
        _gridPhase = GridPhase.Battle;
        SetSelectableTilesAll(true);
        NextTurn();
    }

    public void NextTurn()
    {
        if (_turn == 0 || _turn >= Prep.NumPlayers)
        {
            _turn = 1;
        }
        else
        {
            _turn += 1;
        }
        
        GameInfoDisplay.SetPlayerTurn(_turn);
    }

    public void DisplayAction(UnitAction action, int index=0)
    {
        if (GetUnit(_tileSelected) == null)
        {
            SetSelectableTilesAll(true);
            _stateMachine.RequestStateChange("Idle");
        }
        else
        {
            if (action == UnitAction.Move)
            {
                _stateMachine.RequestStateChange("MoveSelected");
            }
        }
    }

    public void SetAvailableTilesPlacement(int player)
    {
        HashSet<int> selectableTiles = new();
        int start = 0;
        int end = 0;
        if (player == 1)
        {
            start = 1;
            end = X * Prep.NumSpawnRows;
        }
        else if (player == 2)
        {
            end = X * Y;
            start = end - (X * Prep.NumSpawnRows) + 1;
        }
        for (int i = start; i <= end; i++)
        {
            selectableTiles.Add(i);
        }
        SetSelectableTiles(selectableTiles, true);
    }

    public Unit GetUnit(int index)
    {
        return _units.GetValue(index);
    }

    void StateOnEnterIdle()
    {
        Debug.Log("HandleStateIdle");
        SetSelectableTilesAll(true);
    }

    void StateOnEnterTileSelected()
    {
        Debug.Log("HandleStateTileSelected");
    }

    void StateOnEnterMoveSelected()
    {
        Debug.Log("HandleStateMoveSelected");
        SetSelectableTiles(_tiles.GetIndices(_movementHandler.GetMovePositions(_tileSelected, _units, _tiles)), true);
    }

    void HandleUnitPlacement()
    {
        Unit unit = _units.GetValue(_tileHovered);
        if (unit != null && _tiles.GetValue(_tileHovered).Selectable)
        {
            _units.SetValue(_tileHovered, null);
            _unitDragging = unit;
        }
    }

    void SetTileSelected(int index)
    {
        Tile prevTileSelected = _tiles.GetValue(_tileSelected);
        if (prevTileSelected != null) prevTileSelected.State = TileState.Default;

        _tileSelected = index;
        Tile newTileSelected = _tiles.GetValue(_tileSelected);
        if (newTileSelected != null) newTileSelected.State = TileState.Selected;

        UnitInfoDisplay.ShowUnitActions(_units.GetValue(_tileSelected));
    }

    void HandleTileSelect()
    {
        Tile tile = _tiles.GetValue(_tileHovered);
        if (tile != null)
        {
            if (tile.State == TileState.Selected) SetTileSelected(0);
            else                                  SetTileSelected(_tileHovered);
        }
    }

    void HandleMoveSelect()
    {
        Tile tile = _tiles.GetValue(_tileHovered);
        if (tile != null)
        {
            if (tile.Selectable) MoveUnit(_tileSelected, _tileHovered);
        }
        SetTileSelected(0);
    }

    void HandleUnitDrag()
    {
        if (_unitDragging != null)
        {
            ActionBase action = _inputHandlerDragDrop.HandleInput();
            action?.Execute(_unitDragging.gameObject);
        }
    }

    // Helper Methods
    void SetSelectableTiles(HashSet<int> tiles, bool selectable)
    {
        for (int i = 1; i < _tiles.Size(); i++)
        {
            if (tiles.Contains(i)) _tiles.GetValue(i).Selectable = selectable;
            else                   _tiles.GetValue(i).Selectable = !selectable;
        }
    }

    void SetSelectableTilesAll(bool selectable)
    {
        for (int i = 1; i < _tiles.Size(); i++)
        {
            _tiles.GetValue(i).Selectable = selectable;
        }
    }

    bool PlaceUnit(Unit unit, int index)
    {
        if (!_tiles.IsValidIndex(index))
        {
            Debug.Log("PlaceUnit - invalid position");
            DeleteUnit(unit);
            return false;
        }
        if (!_tiles.GetValue(index).Selectable)
        {
            Debug.Log("PlaceUnit - tile not selectable");
            if (unit.Stats.GridPosition > 0)
            {
                SetUnitGridPosition(unit, unit.Stats.GridPosition);
                return true;
            }
            DeleteUnit(unit);
            return false;
        }
        if (_units.GetValue(index) != null)
        {
            Debug.Log("PlaceUnit - tile is occupied");
            if (unit.Stats.GridPosition > 0)
            {
                SetUnitGridPosition(unit, unit.Stats.GridPosition);
                return true;
            }
            DeleteUnit(unit);
            return false;
        }
        SetUnitGridPosition(unit, index);
        return true;
    }

    void SetUnitGridPosition(Unit unit, int index)
    {
        if (_units.SetValue(index, unit)) 
        {
            unit.SetPosition(index, _units.GetVector(index), Visual.TileScale);
            _playerManager.AddPlayerUnit(unit);
        }
    }

    void DeleteUnit(int index)
    {
        Unit unit = _units.GetValue(index);
        if (unit != null) 
        {
            DeleteUnit(unit);
        }
    }

    void DeleteUnit(Unit unit)
    {
        _playerManager.DeletePlayerUnit(unit);
        Destroy(unit.gameObject);
    }

    void MoveUnit(int srcIndex, int dstIndex)
    {
        Unit unit = _units.GetValue(srcIndex);
        if (unit != null) {
            _units.MoveValue(srcIndex, dstIndex);
            SetUnitGridPosition(unit, dstIndex);
        }
    }
}
