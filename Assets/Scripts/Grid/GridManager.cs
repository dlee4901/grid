using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    List<Tile> _tiles;
    List<Unit> _units;
    int _tileHovered;

    public int x;
    public int y;
    public float tileScale;
    public int unitCap;
    public int numPlayers;
    public int numSpawnRows;
    public Sprite tileSprite;

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
        Tile tile_ = Util.CreateGameObject<Tile>();
        _tiles = new();
        _units = new();
        for (int j = 0; j < y; j++)
        {
            for (int i = 0; i < x; i++)
            {
                Tile tile = Instantiate(tile_, GetWorldPos(new Vector2Int(i, j)), Quaternion.identity, transform);
                tile.Init(tileSprite);
                tile.transform.localScale = new Vector3(tileScale, tileScale, transform.localScale.z);
                tile.Id = Flatten(i, j);
                _tiles.Add(tile);
                _units.Add(null);
            }
        }
        _tileHovered = -1;
        //TestGrid();
    }

    void TileHover(int id)
    {
        _tileHovered = id;
    }

    void UnitPlace(Unit unit, int listUIPosition)
    {
        if (AddUnit(Unflatten(_tileHovered), unit))
        {
            EventManager.Singleton.StartUnitUIDragEvent(unit.properties.controller, listUIPosition, true);
        }
    }

    public void OnSelect(InputAction.CallbackContext ctx) 
    {
        if (ctx.started && _tileHovered > -1 && _tiles[_tileHovered].Available)
        {
            Debug.Log("OnSelect");
            Debug.Log(_tileHovered);
        }
    }

    public void SetInitialUnitSpawns(int player)
    {
        List<int> availableTiles = new List<int>();
        int start = -1;
        int end = -1;
        if (player == 1)
        {
            start = 0;
            end = x * numSpawnRows;
        }
        else if (player == 2)
        {
            end = x * y;
            start = end - (x * numSpawnRows);
        }
        for (int i = start; i < end; i++)
        {
            availableTiles.Add(i);
        }
        SetAvailableTiles(availableTiles, true);
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
        int distance = unit.properties.movement.distance;
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
        var movement = unit.properties.movement;
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
            switch (unit.properties.facing)
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
        for (int i = 0; i < _tiles.Count; i++)
        {
            if (tiles.Count > 0 && tiles[0] < x * y && i == tiles[0])
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

    bool AddUnit(Vector2Int position, Unit unit)
    {
        if (!IsValidPosition(position))
        {
            Debug.Log("AddUnit - invalid position");
            Destroy(unit.gameObject);
            return false;
        }
        if (!_tiles[Flatten(position)].Available)
        {
            Debug.Log("AddUnit - tile not available");
            Destroy(unit.gameObject);
            return false;
        }
        _units.Insert(Flatten(position), unit);
        Unit curUnit = _units[Flatten(position)];
        curUnit.transform.position = GetWorldPos(position);
        curUnit.GetComponent<SpriteRenderer>().sortingLayerName = "Unit";
        return true;
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

    Vector3 GetWorldPos(Vector2Int position)
    {
        float xPos = position.x * tileScale / 0.1f;
        float yPos = position.y * tileScale / 0.1f;
        return new Vector3(xPos, yPos, 0);
    }

    bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < x && position.y < y;
    }

    int Flatten(Vector2Int position)
    {
        return Flatten(position.x, position.y);
    }

    int Flatten(int xPos, int yPos)
    {
        return yPos * x + xPos;
    }

    Vector2Int Unflatten(int position)
    {
        return new Vector2Int(position % x, position / x);
    }
}
