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
    public int NumPlayers;
    public int UnitSpawnCap;
    public int NumSpawnRows;
}

public class GridManager : MonoBehaviour
{
    Position<Tile> _tiles;
    Position<Unit> _units;
    MovementHandler _movementHandler;
    InputHandler _inputHandlerDragDrop;
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
        _stateMachine.AddState("Idle", onEnter => Debug.Log("Idle"));
        _stateMachine.AddState("TileSelected", onEnter => Debug.Log("TileSelected"));
        _stateMachine.AddState("ActionSelected", onEnter => Debug.Log("ActionSelected"));
        _stateMachine.SetStartState("Idle");
        _stateMachine.AddTwoWayTransition("Idle", "TileSelected", transition => _tileSelected != 0);
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
        EventManager.Singleton.StartUnitUIUpdateEvent(unit.stats.controller, unit.listUIPosition, PlaceUnit(unit, _tileHovered));
    }

    public void OnSelect(InputAction.CallbackContext ctx) 
    {
        // On Mouse Press
        if (ctx.started)
        {
            if (_gridPhase == GridPhase.Placement)
            {
                HandleUnitPlacement();
            }
            else if (_gridPhase == GridPhase.Battle)
            {
                HandleTileSelect();
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
                _stateMachine.RequestStateChange("ActionSelected");
                SetSelectableTiles(_tiles.GetIndices(_movementHandler.GetMovePositions(_tileSelected, _units, _tiles)), true);
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

    void HandleUnitPlacement()
    {
        Unit unit = _units.GetValue(_tileHovered);
        if (unit != null && _tiles.GetValue(_tileHovered).Selectable)
        {
            _units.SetValue(_tileHovered, null);
            _unitDragging = unit;
        }
    }

    void HandleTileSelect()
    {
        Tile tile = _tiles.GetValue(_tileHovered);
        if (tile != null)
        {
            if (tile.State == TileState.Selected)
            {
                tile.State = TileState.Default;
                _tileSelected = 0;
            }
            else
            {
                Tile prevTile = _tiles.GetValue(_tileSelected);
                if (prevTile != null) prevTile.State = TileState.Default;
                tile.State = TileState.Selected;
                _tileSelected = _tileHovered;
            }
            UnitInfoDisplay.ShowUnitActions(_units.GetValue(_tileSelected));
        }
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
            Destroy(unit.gameObject);
            return false;
        }
        if (!_tiles.GetValue(index).Selectable)
        {
            Debug.Log("PlaceUnit - tile not selectable");
            if (unit.stats.position > 0)
            {
                AddUnitToGrid(unit, unit.stats.position);
                return true;
            }
            Destroy(unit.gameObject);
            return false;
        }
        if (_units.GetValue(index) != null)
        {
            Debug.Log("PlaceUnit - tile is occupied");
            if (unit.stats.position > 0)
            {
                AddUnitToGrid(unit, unit.stats.position);
                return true;
            }
            Destroy(unit.gameObject);
            return false;
        }
        AddUnitToGrid(unit, index);
        return true;
    }

    void AddUnitToGrid(Unit unit, int index)
    {
        if (_units.SetValue(index, unit)) unit.SetPosition(index, _units.GetVector(index), Visual.TileScale);
    }

    // void MoveUnit(int src, int dst)
    // {
    //     if (_units[src] != null) {
    //         _units[dst] = _units[src];
    //         _units[src] = null;
    //     }
    // }
}
