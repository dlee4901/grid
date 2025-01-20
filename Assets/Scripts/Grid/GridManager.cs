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
    public int MovePoints; // 2
    public int Mana; // 3
}

public class GridManager : MonoBehaviour
{
    Position<Tile> _tiles;
    Position<Entity> _entities;
    InputHandler _inputHandlerDragDrop;
    
    StateMachine _stateMachine;

    GridPhase _gridPhase;
    Unit _unitDragging;
    int _tileHovered;
    int _tileSelected;

    public PlayerManager PlayerManager { get; set; }
    public int PlayerTurn { get; set; }

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
        _entities ??= new Position<Entity>(X, Y);
        _inputHandlerDragDrop ??= new InputHandler(InputActionPreset.DragDrop);
        PlayerManager = new PlayerManager(Prep.NumPlayers, Prep.MovePoints, Prep.Mana);
        InitStateMachine();

        _gridPhase = GridPhase.Placement;
        _unitDragging = null;
        _tileHovered = 0;
        _tileSelected = 0;
        PlayerTurn = 0;
        
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
                _tiles.Add(tile);
                _entities.Add(null);
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
        EventManager.Singleton.StartUnitUIUpdateEvent(unit.PlayerController, unit.ListUIPosition, PlaceEntity(unit, _tileHovered));
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
        SetSelectableTilesAll();
        NextTurn();
    }

    public void NextTurn()
    {
        if (PlayerTurn == 0 || PlayerTurn >= Prep.NumPlayers)
        {
            PlayerTurn = 1;
        }
        else
        {
            PlayerTurn += 1;
        }
        SetTileSelected(0);
        PlayerManager.ResetPlayerPoints(PlayerTurn);
        GameInfoDisplay.UpdateDisplay();
        UnitInfoDisplay.UpdateDisplay();
    }

    public void DisplayAction(UnitAction action, int index=0)
    {
        if (GetEntity(_tileSelected) == null)
        {
            SetSelectableTilesAll();
            _stateMachine.RequestStateChange("Idle");
        }
        else
        {
            if (_stateMachine.ActiveStateName == "MoveSelected")
            {
                SetTileSelected(0);
            }
            else if (action == UnitAction.Move)
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
        SetSelectableTiles(selectableTiles);
    }

    public Entity GetEntity(int index)
    {
        return _entities.Get(index);
    }

    public Player GetActivePlayer()
    {
        return PlayerManager.GetPlayer(PlayerTurn);
    }

    void StateOnEnterIdle()
    {
        Debug.Log("StateOnEnterIdle");
        SetSelectableTilesAll();
    }

    void StateOnEnterTileSelected()
    {
        Debug.Log("StateOnEnterTileSelected");
    }

    void StateOnEnterMoveSelected()
    {
        Debug.Log("StateOnEnterMoveSelected");
        Entity entity = _entities.Get(_tileSelected);
        if (entity != null && entity is Unit)
        {
            Unit unit = (Unit)entity;
            SetSelectableTiles(unit.Move.GetTiles(_tileSelected, _tiles, _entities));
        }
        // TEMP
        var fields = Util.GetFields(entity);
        foreach (var field in fields)
        {
            Debug.Log(field);
        }
        //
    }

    void HandleUnitPlacement()
    {
        Entity entity = _entities.Get(_tileHovered);
        Debug.Log("HandleUnitPlacement");
        if (entity != null && _tiles.Get(_tileHovered).Selectable && entity is Unit)
        {
            _entities.Set(_tileHovered, null);
            _unitDragging = (Unit)entity;
        }
    }

    void SetTileSelected(int index)
    {
        Tile prevTileSelected = _tiles.Get(_tileSelected);
        if (prevTileSelected != null) prevTileSelected.State = TileState.Default;

        _tileSelected = index;
        Tile newTileSelected = _tiles.Get(_tileSelected);
        if (newTileSelected != null) newTileSelected.State = TileState.Selected;

        Entity entity = _entities.Get(_tileSelected);
        UnitInfoDisplay.UpdateDisplay(entity);
    }

    void HandleTileSelect()
    {
        Tile tile = _tiles.Get(_tileHovered);
        if (tile != null)
        {
            if (tile.State == TileState.Selected) SetTileSelected(0);
            else                                  SetTileSelected(_tileHovered);
        }
    }

    void HandleMoveSelect()
    {
        Tile tile = _tiles.Get(_tileHovered);
        if (tile != null)
        {
            if (tile.Selectable) MoveEntity(_tileSelected, _tileHovered);
        }
        SetTileSelected(0);
    }

    void HandleUnitDrag()
    {
        if (_unitDragging != null)
        {
            CommandBase command = _inputHandlerDragDrop.HandleInput();
            command?.Execute(_unitDragging.gameObject);
        }
    }

    // Helper Methods
    void SetSelectableTiles(HashSet<int> tiles, bool selectable=true)
    {
        for (int i = 1; i < _tiles.Count(); i++)
        {
            if (i == _tileSelected) Debug.Log(i);
            if (tiles.Contains(i)) _tiles.Get(i).Selectable = selectable;
            else                   _tiles.Get(i).Selectable = !selectable;
        }
    }

    void SetSelectableTilesAll(bool selectable=true, bool setEntityTiles=true)
    {
        for (int i = 1; i < _tiles.Count(); i++)
        {
            _tiles.Get(i).Selectable = selectable;
        }
        if (setEntityTiles) SetEntityTiles();
    }

    void SetEntityTiles()
    {
        for (int i = 1; i < _entities.Count(); i++)
        {
            Entity entity = _entities.Get(i);
            if (entity != null) _tiles.Get(i).Team = entity.PlayerController;
            else                _tiles.Get(i).Team = 0;
        }
    }

    bool PlaceEntity(Entity entity, int index)
    {
        if (!_tiles.IsValidIndex(index))
        {
            Debug.Log("PlaceEntity - invalid position");
            DeleteEntity(entity);
            return false;
        }
        if (!_tiles.Get(index).Selectable)
        {
            Debug.Log("PlaceEntity - tile not selectable");
            if (entity.GridPosition > 0)
            {
                SetEntityGridPosition(entity, entity.GridPosition);
                return true;
            }
            DeleteEntity(entity);
            return false;
        }
        if (_entities.Get(index) != null)
        {
            Debug.Log("PlaceEntity - tile is occupied");
            if (entity.GridPosition > 0)
            {
                SetEntityGridPosition(entity, entity.GridPosition);
                return true;
            }
            DeleteEntity(entity);
            return false;
        }
        SetEntityGridPosition(entity, index);
        return true;
    }

    void SetEntityGridPosition(Entity entity, int index)
    {
        if (_entities.Set(index, entity)) 
        {
            entity.SetPosition(index, _entities.GetVector3(index), Visual.TileScale);
            Debug.Log("SetEntityGridPosition");
            if (entity is Unit) PlayerManager.AddPlayerUnit((Unit)entity);
        }
    }

    void DeleteEntity(int index)
    {
        Entity entity = _entities.Get(index);
        if (entity != null) 
        {
            DeleteEntity(entity);
        }
    }

    void DeleteEntity(Entity entity)
    {
        Debug.Log("DeleteEntity");
        if (entity is Unit) PlayerManager.DeletePlayerUnit((Unit)entity);
        Destroy(entity.gameObject);
    }

    void MoveEntity(int srcIndex, int dstIndex)
    {
        Entity entity = _entities.Get(srcIndex);
        if (entity != null)
        {
            _entities.Move(srcIndex, dstIndex);
            SetEntityGridPosition(entity, dstIndex);
            SetEntityTiles();
            PlayerManager.UpdatePlayerMovePoints(PlayerTurn, -1);
            GameInfoDisplay.UpdateDisplay();
        }
    }
}
