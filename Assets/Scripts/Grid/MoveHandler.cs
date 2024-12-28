using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveHandler
{
    TileSelectionHandler _tileSelectionHandler;

    public MoveHandler()
    {
        _tileSelectionHandler = new TileSelectionHandler();
    }

    public HashSet<int> GetMoves(int index, Position<Tile> tiles, Position<Unit> units)
    {
        HashSet<int> moves = new();
        Vector2Int origin = tiles.GetVector2(index);
        Unit unit = units.Get(index);
        if (origin == Vector2Int.zero || unit == null) return moves;
        foreach (TileSelection move in unit.Moves)
        {
            moves.UnionWith(_tileSelectionHandler.GetSelectableTiles(move, origin, tiles, units));
        }
        moves.Remove(index);
        moves.ExceptWith(units.GetOccupiedIndices());
        return moves;
    }
}