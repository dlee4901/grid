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
    GridPhase _gridPhase;
    Unit _unitDragging;
    int _turn;
    int _tileHovered;
    int _tileSelected;

    DragDropInputHandler _inputHandler;
    StateMachine _stateMachine;

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
        InitGrid();
        InitStateMachine();
    }

    // Update is called once per frame
    void Update()
    {
        HandleUnitDrag();
    }

    void InitGrid()
    {
        _tiles ??= new Position<Tile>(X, Y);
        _units ??= new Position<Unit>(X, Y);
        _gridPhase = GridPhase.Placement;
        _turn = 0;
        _tileHovered = 0;
        _tileSelected = 0;
        _inputHandler = new DragDropInputHandler();
        CreateTiles();
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

    void InitStateMachine()
    {
        _stateMachine = new StateMachine();
        _stateMachine.AddState("Idle");
        _stateMachine.AddState("TileSelected");
        _stateMachine.AddState("ActionSelected");
        _stateMachine.SetStartState("Idle");
        _stateMachine.AddTwoWayTransition("Idle", "TileSelected", transition => _tileSelected != 0);
        _stateMachine.AddTransition("TileSelected", "ActionSelected");
        _stateMachine.AddTransition("ActionSelected", "Idle");
        _stateMachine.Init();
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
        Debug.Log("DisplayAction");
        Debug.Log(_tileSelected);
        Debug.Log(GetUnit(_tileSelected));
        if (GetUnit(_tileSelected) == null)
        {
            SetSelectableTilesAll(true);
            _stateMachine.RequestStateChange("Idle");
            Debug.Log(_stateMachine.ActiveStateName);
        }
        else
        {
            if (action == UnitAction.Move)
            {
                Debug.Log("display unit move");
                _stateMachine.RequestStateChange("ActionSelected");
                Debug.Log(_stateMachine.ActiveStateName);
                SetSelectableTiles(_tiles.GetIndices(GetMovePositions(_tileSelected)), true);
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
        Debug.Log("Tile Selected");
        Debug.Log(_tileSelected);
    }

    void HandleUnitDrag()
    {
        if (_unitDragging != null)
        {
            Debug.Log("HandleUnitDrag");
            ActionBase action = _inputHandler.HandleInput();
            action?.Execute(_unitDragging.gameObject);
        }
    }

    HashSet<Vector2Int> GetMovePositions(int index)
    {
        HashSet<Vector2Int> movePositions = new();
        Unit unit = _units.GetValue(index);
        if (unit == null) return movePositions;
        Vector2Int initialPosition = (Vector2Int)_units.GetVector(index);
        movePositions = GetValidMoves(initialPosition, unit);
        return movePositions;
    }

    HashSet<Vector2Int> GetValidMoves(Vector2Int initialPosition, Unit unit, bool step = false)
    {
        List<List<Vector2Int>> res = new();
        foreach (UnitMovement movement in unit.movement)
        {
            List<Vector2Int> validMoves = new();
            List<Vector2Int> unitVectors = GetUnitVectors(unit, movement);
            int distance = movement.distance;
            if (distance == -1) distance = Math.Max(X, Y);
            if (step)
            {

            }
            else
            {
                for (int i = 0; i < distance; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Vector2Int startPosition = initialPosition;
                        if (i > 0) startPosition = validMoves[8 * (i - 1) + j];
                        Vector2Int targetPosition = startPosition + unitVectors[j];
                        if (_tiles.GetValue(targetPosition) != null) validMoves.Add(targetPosition);
                        else validMoves.Add(startPosition);
                    }
                }
            }
            res.Add(validMoves);
            Debug.Log("GetValidMoves");
            Util.PrintList(new HashSet<Vector2Int>(validMoves));
        }

        List<Vector2Int> combined = new();
        foreach (List<Vector2Int> moves in res)
        {
            combined = combined.Concat(moves).ToList();
        }
        return new HashSet<Vector2Int>(combined);
    }

    List<Vector2Int> GetUnitVectors(Unit unit, UnitMovement movement)
    {
        List<Vector2Int> unitVectors = new();
        List<bool> absoluteDirections = GetAbsoluteDirections(unit, movement);
        for (int i = 0; i < 8; i++)
        {
            int xOffset = 0;
            int yOffset = 0;
            if (absoluteDirections[i])
            {
                if (i > 4)               xOffset = -1;
                else if (i > 0 && i < 4) xOffset = 1;
                if (i > 2 && i < 6)      yOffset = 1;
                else if (i < 2 || i > 6) yOffset = -1;
            }
            unitVectors.Add(new Vector2Int(xOffset, yOffset));
        }
        Debug.Log("GetUnitVectors");
        Util.PrintList(unitVectors);
        return unitVectors;
    }

    List<bool> GetAbsoluteDirections(Unit unit, UnitMovement movement)
    {
        Debug.Log("GetAbsoluteDirections");
        Debug.Log(movement.direction);
        List<bool> absoluteDirections = new List<bool>{false, false, false, false, false, false, false, false};
        switch (movement.direction)
        {
            case Direction.stride: case Direction.line:
                for (int i = 0; i < 8; i++) absoluteDirections[i] = true;
                break;
            case Direction.diagonal:
                for (int i = 1; i < 8; i += 2) absoluteDirections[i] = true;
                break;
            case Direction.step: case Direction.straight:
                for (int i = 0; i < 8; i += 2) absoluteDirections[i] = true;
                break;
            case Direction.horizontal:
                absoluteDirections[2] = true;
                absoluteDirections[6] = true;
                break;
            case Direction.vertical:
                absoluteDirections[0] = true;
                absoluteDirections[4] = true;
                break;
            case Direction.N:
                absoluteDirections[0] = true;
                break;
            case Direction.NE:
                absoluteDirections[1] = true;
                break;
            case Direction.E:
                absoluteDirections[2] = true;
                break;
            case Direction.SE:
                absoluteDirections[3] = true;
                break;
            case Direction.S:
                absoluteDirections[4] = true;
                break;
            case Direction.SW:
                absoluteDirections[5] = true;
                break;
            case Direction.W:
                absoluteDirections[6] = true;
                break;
            case Direction.NW:
                absoluteDirections[7] = true;
                break;
            default:
                Debug.LogError("unit movement is invalid");
                return absoluteDirections;
        }
        if (movement.relativeFacing)
        {
            int shift = 0;
            switch (unit.stats.facing)
            {
                case DirectionFacing.E:
                    shift = 6;
                    break;
                case DirectionFacing.S:
                    shift = 2;
                    break;
                case DirectionFacing.W:
                    shift = 4;
                    break;
                default:
                    Debug.LogError("unit facing is invalid");
                    return absoluteDirections;
            }
            return (List<bool>)absoluteDirections.Skip(shift).Concat(absoluteDirections.Take(shift));
        }
        Util.PrintList(absoluteDirections);
        return absoluteDirections;
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
        Debug.Log("PlaceUnit - unit placed");
        Debug.Log(unit);
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
