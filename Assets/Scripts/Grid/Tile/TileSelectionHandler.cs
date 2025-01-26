using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSelectionHandler
{
    public TileSelectionHandler() {}

    public HashSet<int> GetSelectableTiles(TileSelection tileSelection, Vector2Int origin, Position<Tile> tiles, Position<Entity> entities)
    {
        if (!Util.IsValidOriginAndUnit(origin, tiles, entities)) return null;
        List<Vector2Int> selectableTiles = new();
        Unit unit = (Unit)entities.Get(origin);
        List<Vector2Int> unitVectors = GetUnitVectors(tileSelection, unit);
        List<bool> collisions = new List<bool>{false, false, false, false, false, false, false, false};
        int distance = tileSelection.Distance;
        var (x, y, z) = tiles.Bounds();
        if (distance == -1) distance = Math.Max(x, y);
        if (tileSelection.Direction == Direction.step || tileSelection.Direction == Direction.stride)
        {
            Dictionary<Vector2Int, bool> visitedTiles = new();
            for (int i = 0; i < tiles.Count(); i++) visitedTiles[tiles.GetVector2(i)] = false;
            List<Vector2Int> checkTiles = new() {origin};
            for (int i = 0; i <= distance; i++)
            {
                List<Vector2Int> nextTiles = new();
                foreach (Vector2Int tilePosition in checkTiles)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (tileSelection.Direction == Direction.step && j % 2 == 1) continue;
                        Vector2Int startPosition = tilePosition;
                        Vector2Int targetPosition = startPosition + unitVectors[j];
                        if (visitedTiles[tilePosition] || tiles.Get(targetPosition) == null) continue;
                        Entity entityColliding = entities.Get(targetPosition);
                        if (entityColliding != null)
                        {
                            if (tileSelection.Passthrough == 0
                                || (tileSelection.Passthrough == Team.Ally && !entityColliding.SameController(unit))
                                || (tileSelection.Passthrough == Team.Enemy && entityColliding.SameController(unit)))
                            {
                                continue;
                            }
                        }
                        nextTiles.Add(targetPosition);
                    }
                    visitedTiles[tilePosition] = true;
                }
                selectableTiles.AddRange(checkTiles);
                checkTiles = nextTiles;
            }
        }
        else
        {
            for (int i = 0; i < distance; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Vector2Int startPosition = origin;
                    if (i > 0) startPosition = selectableTiles[8 * (i - 1) + j];
                    if (collisions[j])
                    {
                        selectableTiles.Add(startPosition);
                        continue;
                    }
                    Vector2Int targetPosition = startPosition + unitVectors[j];
                    bool targetPositionValid = true;
                    if (tiles.Get(targetPosition) == null) targetPositionValid = false;
                    else 
                    {
                        Entity entityColliding = entities.Get(targetPosition);
                        if (entityColliding != null)
                        {
                            if (tileSelection.Passthrough == 0
                                || (tileSelection.Passthrough == Team.Ally && !entityColliding.SameController(unit))
                                || (tileSelection.Passthrough == Team.Enemy && entityColliding.SameController(unit)))
                            {
                                collisions[j] = true;
                            }
                        }
                    }
                    if (targetPositionValid) selectableTiles.Add(targetPosition);
                    else selectableTiles.Add(startPosition);
                }
            }
        }
        return tiles.GetIndicesHashSet(selectableTiles);
    }

    public List<Vector2Int> GetUnitVectors(TileSelection tileSelection, Unit unit)
    {
        List<Vector2Int> unitVectors = new();
        List<bool> absoluteDirections = GetAbsoluteDirections(tileSelection, unit);
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

    public List<bool> GetAbsoluteDirections(TileSelection tileSelection, Unit unit)
    {
        List<bool> absoluteDirections = new List<bool>{false, false, false, false, false, false, false, false};
        switch (tileSelection.Direction)
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
        if (tileSelection.RelativeFacing && unit != null)
        {
            int shift = 0;
            switch (unit.DirectionFacing)
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