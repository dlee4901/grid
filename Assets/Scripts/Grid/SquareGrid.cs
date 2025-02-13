using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityHFSM;

public enum GridPhase {Placement, Battle}

public class SquareGrid : MonoBehaviour
{
    [Header("Size")]
    [SerializeField] private int _x;
    [SerializeField] private int _y;
    [SerializeField] private float _tileScale;

    [Header("Properties")]
    [SerializeField] private Sprite _tileSprite;
    [SerializeField] private int _numSpawnRows; // 2
    [SerializeField] private int _unitCostTotal; // 20
    [SerializeField] private int _numPlayers; // 2
    [field: SerializeField] public int MovePoints { get; private set; } // 2
    [field: SerializeField] public int Mana { get; private set; } // 3

    public PlayerManager PlayerManager { get; private set; }
    public int PlayerTurn { get; private set; }

    private UIManager _uiManager;

    private Position<Tile> _tiles;
    private Position<Entity> _entities;
    private InputHandler _inputHandler;
    //private InputHandler _inputHandlerDragDrop;
    
    private StateMachine _stateMachine;

    private GridPhase _gridPhase;
    private Unit _unitDragging;
    private int _tileHovered;
    private int _tileSelected;

    // Start is called before the first frame update
    private void Start()
    {
        _tiles = new Position<Tile>(_x, _y);
        _entities = new Position<Entity>(_x, _y);
        //_inputHandlerDragDrop ??= new InputHandler(InputActionPreset.DragDrop);
        _inputHandler = new InputHandler();
        PlayerManager = new PlayerManager(_numPlayers, MovePoints, Mana);

        _gridPhase = GridPhase.Placement;
        _unitDragging = null;
        _tileHovered = 0;
        _tileSelected = 0;
        PlayerTurn = 0;

        EventManager.Singleton.TileHover += OnEventTileHover;
        EventManager.Singleton.UnitPlace += OnEventUnitPlace;
        EventManager.Singleton.GameInfoDisplayEndTurn += OnEventGameInfoDisplayEndTurn;
        EventManager.Singleton.UnitInfoDisplayMove += OnEventUnitInfoDisplayMove;
        _inputHandler.SelectPerformed += OnInputSelectPerformed;
        
        CreateTiles();
        InitStateMachine();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleUnitDrag();
        _stateMachine.OnLogic();
    }

    public void Init(UIManager uiManager)
    {
        _uiManager = uiManager;
        
    }

    private void OnEventTileHover(int id)
    {
        _tileHovered = id;
    }

    private void OnEventUnitPlace(Unit unit)
    {
        _unitDragging = null;
        EventManager.Singleton.StartUnitUIUpdate(unit.PlayerController, unit.ListUIPosition, PlaceEntity(unit, _tileHovered));
    }

    private void OnEventGameInfoDisplayEndTurn()
    {
        NextTurn();
    }

    private void OnEventUnitInfoDisplayMove()
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
            else
            {
                _stateMachine.RequestStateChange("MoveSelected");
            }
        }
    }

    private void OnInputSelectPerformed()
    {
        if (_tileHovered != 0)
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

    // public void OnSelect(InputAction.CallbackContext ctx) 
    // {
    //     // On Mouse Press on Grid
    //     if (ctx.started && _tileHovered != 0)
    //     {
    //         if (_gridPhase == GridPhase.Placement)
    //         {
    //             HandleUnitPlacement();
    //         }
    //         else if (_gridPhase == GridPhase.Battle)
    //         {
    //             if (_stateMachine.ActiveStateName == "MoveSelected")
    //             {
    //                 HandleMoveSelect();
    //             }
    //             else
    //             {
    //                 HandleTileSelect();
    //             }
    //         }
    //     }
    // }

    private void InitStateMachine()
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

    private void CreateTiles()
    {
        Tile tileGO = Util.CreateGameObject<Tile>();
        for (int j = 1; j <= _y; j++)
        {
            for (int i = 1; i <= _x; i++)
            {
                Tile tile = Instantiate(tileGO, Util.Get2DWorldPos(new Vector3Int(i, j, 0), _tileScale), Quaternion.identity, transform);
                tile.Init(_tileSprite, _tileScale, _tiles.GetIndex(new Vector2Int(i, j)));
                _tiles.Add(tile);
                _entities.Add(null);
            }
        }
        Destroy(tileGO.gameObject);
    }

    public void StartGame()
    {
        _gridPhase = GridPhase.Battle;
        SetSelectableTilesAll();
        NextTurn();
    }

    private void NextTurn()
    {
        if (PlayerTurn == 0 || PlayerTurn >= _numPlayers)
        {
            PlayerTurn = 1;
        }
        else
        {
            PlayerTurn += 1;
        }
        SetTileSelected(0);
        PlayerManager.ResetPlayerPoints(PlayerTurn);
        _uiManager.UpdateGridDisplays(this);
    }

    // public void DisplayAction(UnitAction action)
    // {
    //     if (GetEntity(_tileSelected) == null)
    //     {
    //         SetSelectableTilesAll();
    //         _stateMachine.RequestStateChange("Idle");
    //     }
    //     else
    //     {
    //         if (_stateMachine.ActiveStateName == "MoveSelected")
    //         {
    //             SetTileSelected(0);
    //         }
    //         else if (action == UnitAction.Move)
    //         {
    //             _stateMachine.RequestStateChange("MoveSelected");
    //         }
    //     }
    // }

    public void SetAvailableTilesPlacement(int player)
    {
        HashSet<int> selectableTiles = new();
        int start = 0;
        int end = 0;
        if (player == 1)
        {
            start = 1;
            end = _x * _numSpawnRows;
        }
        else if (player == 2)
        {
            end = _x * _y;
            start = end - (_x * _numSpawnRows) + 1;
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

    public (int, int, float) GetSize()
    {
        return (_x, _y, _tileScale);
    }

    private void StateOnEnterIdle()
    {
        Debug.Log("StateOnEnterIdle");
        SetSelectableTilesAll();
    }

    private void StateOnEnterTileSelected()
    {
        Debug.Log("StateOnEnterTileSelected");
    }

    private void StateOnEnterMoveSelected()
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

    private void HandleUnitPlacement()
    {
        Entity entity = _entities.Get(_tileHovered);
        if (entity != null && _tiles.Get(_tileHovered).Selectable && entity is Unit)
        {
            _entities.Set(_tileHovered, null);
            _unitDragging = (Unit)entity;
        }
    }

    private void SetTileSelected(int index)
    {
        Tile prevTileSelected = _tiles.Get(_tileSelected);
        if (prevTileSelected != null) prevTileSelected.State = TileState.Default;

        _tileSelected = index;
        Tile newTileSelected = _tiles.Get(_tileSelected);
        if (newTileSelected != null) newTileSelected.State = TileState.Selected;

        Entity entity = _entities.Get(_tileSelected);
        _uiManager.UpdateGridDisplays(this, entity);
    }

    private void HandleTileSelect()
    {
        Tile tile = _tiles.Get(_tileHovered);
        if (tile != null)
        {
            if (tile.State == TileState.Selected) SetTileSelected(0);
            else                                  SetTileSelected(_tileHovered);
        }
    }

    private void HandleMoveSelect()
    {
        Tile tile = _tiles.Get(_tileHovered);
        if (tile != null)
        {
            if (tile.Selectable) MoveEntity(_tileSelected, _tileHovered);
        }
        SetTileSelected(0);
    }

    private void HandleUnitDrag()
    {
        // if (_unitDragging != null)
        // {
        //     CommandBase command = _inputHandlerDragDrop.HandleInput();
        //     command?.Execute(_unitDragging.gameObject);
        // }
        if (_unitDragging != null)
        {
            CommandBase command = _inputHandler.GetCommand(CommandPreset.DragDrop);
            command?.Execute(_unitDragging.gameObject);
        }
    }

    // Helper Methods
    private void SetSelectableTiles(HashSet<int> tiles, bool selectable=true)
    {
        for (int i = 1; i < _tiles.Count(); i++)
        {
            if (i == _tileSelected) Debug.Log(i);
            if (tiles.Contains(i)) _tiles.Get(i).Selectable = selectable;
            else                   _tiles.Get(i).Selectable = !selectable;
        }
    }

    private void SetSelectableTilesAll(bool selectable=true, bool setEntityTiles=true)
    {
        for (int i = 1; i < _tiles.Count(); i++)
        {
            _tiles.Get(i).Selectable = selectable;
        }
        if (setEntityTiles) SetEntityTiles();
    }

    private void SetEntityTiles()
    {
        for (int i = 1; i < _entities.Count(); i++)
        {
            Entity entity = _entities.Get(i);
            if (entity != null) _tiles.Get(i).Team = entity.PlayerController;
            else                _tiles.Get(i).Team = 0;
        }
    }

    private bool PlaceEntity(Entity entity, int index)
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

    private void SetEntityGridPosition(Entity entity, int index)
    {
        if (_entities.Set(index, entity)) 
        {
            entity.SetPosition(index, _entities.GetVector3(index), _tileScale);
            Debug.Log("SetEntityGridPosition");
            if (entity is Unit) PlayerManager.AddPlayerUnit((Unit)entity);
        }
    }

    private void DeleteEntity(int index)
    {
        Entity entity = _entities.Get(index);
        if (entity != null) 
        {
            DeleteEntity(entity);
        }
    }

    private void DeleteEntity(Entity entity)
    {
        Debug.Log("DeleteEntity");
        if (entity is Unit) PlayerManager.DeletePlayerUnit((Unit)entity);
        Destroy(entity.gameObject);
    }

    private void MoveEntity(int srcIndex, int dstIndex)
    {
        Entity entity = _entities.Get(srcIndex);
        if (entity != null)
        {
            _entities.Move(srcIndex, dstIndex);
            SetEntityGridPosition(entity, dstIndex);
            SetEntityTiles();
            PlayerManager.UpdatePlayerMovePoints(PlayerTurn, -1);
            _uiManager.UpdateGridDisplays(this);
        }
    }
}
