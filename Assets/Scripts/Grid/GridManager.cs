using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //[SerializeField] GridProperties gridProperties;

    public int _x;
    public int _y;
    public float _tileScale;
    public int _unitCap;
    public int _numPlayers;
    List<Tile> _tiles;
    List<Unit> _units;

    [SerializeField] Tile _tile;

    public int _tileHovered;

    // Start is called before the first frame update
    void Start()
    {
        InitGrid();
        EventManager.Singleton.TileHoverEvent += TileHover;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_tileHovered);
    }

    void TileHover(int id)
    {
        _tileHovered = id;
    }

    void InitGrid()
    {
        _tiles = new();
        _units = new();
        for (int j = 0; j < _y; j++)
        {
            for (int i = 0; i < _x; i++)
            {
                float x = i * _tileScale / 0.1f;
                float y = j * _tileScale / 0.1f;
                var tile_ = Instantiate(_tile, new Vector3(x, y, 0), Quaternion.identity, this.transform);
                tile_.transform.localScale = new Vector3(_tileScale, _tileScale, transform.localScale.z);
                tile_.Id = Flatten(i, j);
                _tiles.Add(tile_);
                _units.Add(null);
                //Debug.Log(i + " " + j + " " + Flatten(i, j) + " " + Unflatten(Flatten(i, j)));
                //Debug.Log(_tiles[Flatten(i, j)].Id);
            }
        }
        //TestGrid();
    }

    // void TestGrid()
    // {
    //     var rookMove = UnitMovement.Create(Direction.straight, -1, false, false);
    //     var bishopMove = UnitMovement.Create(Direction.diagonal, -1, false, false);

    //     var rook1 = Unit.Create("rook1", rookMove);
    //     var bishop1 = Unit.Create("bishop1", bishopMove);
    //     var rook2 = Unit.Create("rook2", rookMove);
    //     var bishop2 = Unit.Create("bishop2", bishopMove);

    //     ValidateDeployPositions();
    //     var deployPositions1 = gridProperties.deployPositions[0];
    //     var deployPositions2 = gridProperties.deployPositions[1];

    //     AddUnit(deployPositions1.positions[0], rook1);
    //     AddUnit(deployPositions1.positions[1], bishop1);

    //     AddUnit(deployPositions2.positions[0], rook2);
    //     AddUnit(deployPositions2.positions[1], bishop2);
    // }

    // void ValidateDeployPositions()
    // {
    //     foreach (var deployPositions in gridProperties.deployPositions)
    //     {
    //         for (int i = 0; i < deployPositions.positions.Count; i++)
    //         {
    //             if (!IsValidPosition(deployPositions.positions[i]))
    //             {
    //                 deployPositions.positions.RemoveAt(i);
    //                 i--;
    //             }
    //         }
    //     }
    // }

    HashSet<Vector2Int> GetMovePositions(Vector2Int position)
    {
        HashSet<Vector2Int> movePositions = new();
        Unit unit = GetUnit(position);
        if (!IsValidPosition(position) || unit == null) return movePositions;
        movePositions = GetValidMoves(position, unit);
        return movePositions;
    }

    HashSet<Vector2Int> GetValidMoves(Vector2Int initialPosition, Unit unit, bool step = false)
    {
        List<Vector2Int> validMoves = new();
        List<Vector2Int> unitVectors = GetUnitVectors(unit);
        int distance = unit.movement.distance;
        if (distance == -1) distance = Math.Max(_x, _y);
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
                    if (IsValidPosition(targetPosition) && GetUnit(targetPosition) != null) validMoves.Add(targetPosition);
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
            int x = 0;
            int y = 0;
            if (absoluteDirections[i])
            {
                if (i > 4)               x = -1;
                else if (i > 0 && i < 4) x = 1;
                if (i > 2 && i < 6)      y = 1;
                else if (i < 2 || i > 6) y = -1;
            }
            unitVectors.Add(new Vector2Int(x, y));
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
            switch (unit.facing)
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
    void AddUnit(Vector2Int position, Unit unit)
    {
        if (!IsValidPosition(position))
        {
            Debug.LogError("getting invalid position");
        }
        else
        {
            Debug.Log(Flatten(position));
            _units.Insert(Flatten(position), unit);
        }
    }

    void MoveUnit(int src, int dst)
    {
        if (_units[src] != null) {
            _units[dst] = _units[src];
            _units[src] = null;
        }
    }

    Unit GetUnit(Vector2Int position)
    {
        if (!IsValidPosition(position))
        {
            Debug.LogError("getting invalid position");
            return null;
        }
        return _units[Flatten(position)];
    }

    bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < _x && position.y < _y;
    }

    int Flatten(Vector2Int position)
    {
        return Flatten(position.x, position.y);
    }

    int Flatten(int x, int y)
    {
        return y * _x + x;
    }

    Vector2Int Unflatten(int position)
    {
        return new Vector2Int(position % _x, position / _x);
    }
}
