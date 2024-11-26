using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GridPhase {Placement, Battle}

[Serializable]
public struct GridVisual
{
    public Sprite tileSprite;
    public float tileScale;
}

[Serializable]
public struct GridPrep
{
    public int numPlayers;
    public int unitSpawnCap;
    public int numSpawnRows;
}

public class GridManager : MonoBehaviour
{
    Position<Tile> _tiles;
    Position<Unit> _units;
    int _tileHovered;
    int _turn;
    GridPhase _gridPhase;
    UnitInputHandler _inputHandler;

    // TODO: integrate into state machine
    bool _tileSelected;
    bool _unitSelected;
    Unit _unitDragging;

    public int x;
    public int y;
    public GridVisual visual;
    public GridPrep prep;
    public GameInfoDisplay gameInfoDisplay;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Singleton.EndTurnEvent += EndTurn;
        EventManager.Singleton.TileHoverEvent += TileHover;
        EventManager.Singleton.UnitPlaceEvent += UnitPlace;
        InitGrid();
    }

    // Update is called once per frame
    void Update()
    {
        HandleUnitDrag();
    }

    void InitGrid()
    {
        _tiles = new Position<Tile>(x, y);
        _units = new Position<Unit>(x, y);
        _tileHovered = 0;
        _turn = 0;
        _gridPhase = GridPhase.Placement;
        _inputHandler = new UnitInputHandler(InputSystem.actions.FindAction("Player/Select"));
        
        CreateTiles();
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
        if (ctx.started && _tileHovered > 0 && _tiles.GetValue(_tileHovered).Available)
        {
            Debug.Log("OnSelect");
            Debug.Log(_tileHovered);
            HandleTileSelect();
        }
    }

    void CreateTiles()
    {
        Tile tileGO = Util.CreateGameObject<Tile>();
        for (int j = 1; j <= y; j++)
        {
            for (int i = 1; i <= x; i++)
            {
                Tile tile = Instantiate(tileGO, Util.Get2DWorldPos(new Vector3Int(i, j, 0), visual.tileScale), Quaternion.identity, transform);
                tile.Init(visual.tileSprite, visual.tileScale, _tiles.GetIndex(new Vector2Int(i, j)));
                _tiles.AddValue(tile);
                _units.AddValue(null);
            }
        }
        Destroy(tileGO.gameObject);
    }

    public void StartGame()
    {
        _gridPhase = GridPhase.Battle;
        SetAvailableTilesAll(true);
        NextTurn();
    }

    public void EndTurn()
    {
        NextTurn();
    }

    void NextTurn()
    {
        if (_turn == 0 || _turn >= prep.numPlayers)
        {
            _turn = 1;
        }
        else
        {
            _turn += 1;
        }
        gameInfoDisplay.SetPlayerTurn(_turn);
    }

    public void SetAvailableTilesPlacement(int player)
    {
        List<int> availableTiles = new List<int>();
        int start = 0;
        int end = 0;
        if (player == 1)
        {
            start = 1;
            end = x * prep.numSpawnRows;
        }
        else if (player == 2)
        {
            end = x * y;
            start = end - (x * prep.numSpawnRows) + 1;
        }
        for (int i = start; i <= end; i++)
        {
            availableTiles.Add(i);
            Debug.Log(i);
        }
        SetAvailableTiles(availableTiles, true);
    }

    void HandleUnitDrag()
    {
        if (_unitDragging != null)
        {
            ActionBase action = _inputHandler.HandleInput(_unitDragging);
            if (action != null) action.Execute();
        }
    }

    void HandleTileSelect()
    {
        if (_gridPhase == GridPhase.Placement)
        {
            Unit unit = _units.GetValue(_tileHovered);
            if (unit != null)
            {
                _units.SetValue(_tileHovered, null);
                _unitDragging = unit;
            }
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
        List<Vector2Int> validMoves = new();
        List<Vector2Int> unitVectors = GetUnitVectors(unit);
        int distance = unit.movement.distance;
        if (distance == -1) distance = Math.Max(x, y);
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
                    if (_units.GetValue(targetPosition) != null) validMoves.Add(targetPosition);
                    else validMoves.Add(startPosition);
                }
            }
        }
        return new HashSet<Vector2Int>(validMoves);
    }

    List<Vector2Int> GetUnitVectors(Unit unit)
    {
        List<Vector2Int> unitVectors = new();
        List<bool> absoluteDirections = GetAbsoluteDirections(unit);
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
        return unitVectors;
    }

    List<bool> GetAbsoluteDirections(Unit unit)
    {
        List<bool> absoluteDirections = new List<bool>{false, false, false, false, false, false, false, false};
        var movement = unit.movement;
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
        return absoluteDirections;
    }

    // Helper Methods
    void SetAvailableTiles(List<int> tiles, bool available)
    {
        tiles.Sort();
        for (int i = 1; i < _tiles.Size(); i++)
        {
            if (tiles.Count > 0 && tiles[0] >= 1 && tiles[0] <= x * y && i == tiles[0])
            {
                _tiles.GetValue(i).Available = available;
                tiles.RemoveAt(0);
            }
            else
            {
                _tiles.GetValue(i).Available = !available;
            }
        }
    }

    void SetAvailableTilesAll(bool available)
    {
        for (int i = 1; i < _tiles.Size(); i++)
        {
            _tiles.GetValue(i).Available = available;
        }
    }

    bool PlaceUnit(Unit unit, int idx)
    {
        if (!_tiles.IsValidIndex(idx))
        {
            Debug.Log("AddUnit - invalid position");
            Destroy(unit.gameObject);
            return false;
        }
        if (!_tiles.GetValue(idx).Available)
        {
            Debug.Log("AddUnit - tile not available");
            if (unit.stats.position > 0)
            {
                AddUnitToGrid(unit, unit.stats.position);
                return true;
            }
            Destroy(unit.gameObject);
            return false;
        }
        if (_units.GetValue(idx) != null)
        {
            Debug.Log("AddUnit - tile is occupied");
            if (unit.stats.position > 0)
            {
                AddUnitToGrid(unit, unit.stats.position);
                return true;
            }
            Destroy(unit.gameObject);
            return false;
        }
        AddUnitToGrid(unit, idx);
        return true;
    }

    void AddUnitToGrid(Unit unit, int idx)
    {
        _units.SetValue(idx, unit);
        unit.SetPosition(idx, _units.GetVector(idx), visual.tileScale);
    }

    // void MoveUnit(int src, int dst)
    // {
    //     if (_units[src] != null) {
    //         _units[dst] = _units[src];
    //         _units[src] = null;
    //     }
    // }
}
