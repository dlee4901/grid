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
    List<Tile> _tiles;
    List<Unit> _units;
    int _tileHovered;
    int _turn;
    GridPhase _gridPhase;
    Position _position;

    public int x;
    public int y;
    public GridVisual visual;
    public GridPrep prep;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Singleton.TileHoverEvent += TileHover;
        EventManager.Singleton.UnitPlaceEvent += UnitPlace;
        InitGrid();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_tileHovered);
    }

    void InitGrid()
    {
        _tiles = new() {null};
        _units = new() {null};
        _tileHovered = 0;
        _turn = 0;
        _gridPhase = GridPhase.Placement;
        _position = new Position(x, y);
        
        CreateTiles();
    }

    void TileHover(int id)
    {
        _tileHovered = id;
    }

    void UnitPlace(Unit unit, int listUIPosition)
    {
        EventManager.Singleton.StartUnitUIUpdateEvent(unit.stats.controller, listUIPosition, PlaceUnit(unit, _tileHovered));
    }

    public void OnSelect(InputAction.CallbackContext ctx) 
    {
        if (ctx.started && _tileHovered > 0 && _tiles[_tileHovered].Available)
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
                Tile tile = Instantiate(tileGO, _position.Get2DWorldPos(new Vector3Int(i, j, 1), visual.tileScale), Quaternion.identity, transform);
                tile.Init(visual.tileSprite, visual.tileScale, _position.GetIndex(i, j));
                _tiles.Add(tile);
                _units.Add(null);
            }
        }
        Destroy(tileGO.gameObject);
    }

    public void StartGame()
    {
        _gridPhase = GridPhase.Battle;
        SetAvailableTilesAll(true);
        StartTurn();
    }

    void StartTurn()
    {
        if (_turn == 0 || _turn >= prep.numPlayers)
        {
            _turn = 1;
        }
        else
        {
            _turn += 1;
        }
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

    void HandleTileSelect()
    {
        if (_gridPhase == GridPhase.Placement)
        {
            Unit unit = _units[_tileHovered];
            if (unit != null)
            {
                _units[_tileHovered] = null;
                unit.isDragging = true;
            }
        }
    }

    HashSet<Vector2Int> GetMovePositions(int index)
    {
        HashSet<Vector2Int> movePositions = new();
        Unit unit = GetUnit(index);
        if (!_position.IsValidIndex(index) || unit == null) return movePositions;
        Vector2Int initialPosition = (Vector2Int)_position.GetVector(index);
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
                    if (_position.IsValidVector(targetPosition) && GetUnit(_position.GetIndex(targetPosition)) != null) validMoves.Add(targetPosition);
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
        for (int i = 1; i < _tiles.Count; i++)
        {
            if (tiles.Count > 0 && tiles[0] >= 1 && tiles[0] <= x * y && i == tiles[0])
            {
                _tiles[i].Available = available;
                tiles.RemoveAt(0);
            }
            else
            {
                _tiles[i].Available = !available;
            }
        }
    }

    void SetAvailableTilesAll(bool available)
    {
        for (int i = 1; i < _tiles.Count; i++)
        {
            _tiles[i].Available = available;
        }
    }

    bool PlaceUnit(Unit unit, int idx)
    {
        if (!_position.IsValidIndex(idx))
        {
            Debug.Log("AddUnit - invalid position");
            Destroy(unit.gameObject);
            return false;
        }
        if (!_tiles[idx].Available)
        {
            Debug.Log("AddUnit - tile not available");
            if (unit.stats.position > 0)
            {
                AddUnit(unit, unit.stats.position);
                return true;
            }
            Destroy(unit.gameObject);
            return false;
        }
        Debug.Log(_tiles[idx]);
        if (_units[idx] != null)
        {
            Debug.Log("AddUnit - tile is occupied");
            if (unit.stats.position > 0)
            {
                AddUnit(unit, unit.stats.position);
                return true;
            }
            Destroy(unit.gameObject);
            return false;
        }
        AddUnit(unit, idx);
        return true;
    }

    void AddUnit(Unit unit, int index)
    {
        _units[index] = unit;
        unit.stats.position = index;
        unit.transform.position = _position.Get2DWorldPos(_position.GetVector(index), visual.tileScale);
    }

    void MoveUnit(int src, int dst)
    {
        if (_units[src] != null) {
            _units[dst] = _units[src];
            _units[src] = null;
        }
    }

    Unit GetUnit(int index)
    {
        if (_position.IsValidIndex(index))
        {
            Debug.LogError("getting invalid position");
            return null;
        }
        return _units[index];
    }

    // Vector3 GetWorldPos(Vector2Int position)
    // {
    //     float xPos = position.x * visual.tileScale / 0.1f;
    //     float yPos = position.y * visual.tileScale / 0.1f;
    //     return new Vector3(xPos, yPos, 0);
    // }

    // bool IsValidPosition(Vector2Int position)
    // {
    //     return position.x >= 0 && position.y >= 0 && position.x < x && position.y < y;
    // }

    // int Flatten(Vector2Int position)
    // {
    //     return Flatten(position.x, position.y);
    // }

    // int Flatten(int xPos, int yPos)
    // {
    //     return yPos * x + xPos;
    // }

    // Vector2Int Unflatten(int position)
    // {
    //     return new Vector2Int(position % x, position / x);
    // }
}
