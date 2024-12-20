using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveHandler
{
    public TileSelection TileSelection;

    public MoveHandler(TileSelection tileSelection=null)
    {
        TileSelection = TileSelection != null ? TileSelection : tileSelection;
    }

    public HashSet<Vector2Int> GetMovePositions(int index, Position<Unit> units, Position<Tile> tiles)
    {
        HashSet<Vector2Int> movePositions = new();
        Unit unit = units.Get(index);
        if (unit == null) return movePositions;
        Vector2Int initialPosition = (Vector2Int)units.GetVector(index);
        movePositions = GetValidMoves(initialPosition, units, tiles);
        movePositions.Remove(initialPosition);
        return movePositions;
    }

    public HashSet<Vector2Int> GetValidMoves(Vector2Int initialPosition, Position<Unit> units, Position<Tile> tiles)
    {
        List<List<Vector2Int>> res = new();
        Unit unit = units.Get(initialPosition);
        foreach (UnitMove move in unit.Moves)
        {
            List<Vector2Int> validMoves = new();
            List<Vector2Int> unitVectors = GetUnitVectors(unit, move);
            List<bool> collisions = new List<bool>{false, false, false, false, false, false, false, false};
            int distance = move.Distance;
            var (x, y, z) = tiles.Bounds();
            if (distance == -1) distance = Math.Max(x, y);
            if (move.Direction == Direction.step || move.Direction == Direction.stride)
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
                        if (collisions[j])
                        {
                            validMoves.Add(startPosition);
                            continue;
                        }
                        Vector2Int targetPosition = startPosition + unitVectors[j];
                        bool targetPositionValid = true;
                        if (tiles.Get(targetPosition) == null) targetPositionValid = false;
                        else {
                            Unit unitColliding = units.Get(targetPosition);
                            if (unitColliding != null)
                            {
                                if (move.Passthrough == Passthrough.None 
                                    || (move.Passthrough == Passthrough.Ally && !unitColliding.SameController(unit))
                                    || (move.Passthrough == Passthrough.Enemy && unitColliding.SameController(unit)))
                                {
                                    targetPositionValid = false;
                                    collisions[j] = true;
                                }
                            }
                        }
                        if (targetPositionValid) validMoves.Add(targetPosition);
                        else validMoves.Add(startPosition);
                    }
                }
            }
            res.Add(validMoves);
        }

        List<Vector2Int> combined = new();
        foreach (List<Vector2Int> moves in res)
        {
            combined = combined.Concat(moves).ToList();
        }
        return new HashSet<Vector2Int>(combined);
    }

    public List<Vector2Int> GetUnitVectors(Unit unit, UnitMove move)
    {
        List<Vector2Int> unitVectors = new();
        List<bool> absoluteDirections = GetAbsoluteDirections(unit, move);
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

    public List<bool> GetAbsoluteDirections(Unit unit, UnitMove move)
    {
        List<bool> absoluteDirections = new List<bool>{false, false, false, false, false, false, false, false};
        switch (move.Direction)
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
                Debug.LogError("unit move is invalid");
                return absoluteDirections;
        }
        if (move.RelativeFacing)
        {
            int shift = 0;
            switch (unit.Stats.DirectionFacing)
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
}