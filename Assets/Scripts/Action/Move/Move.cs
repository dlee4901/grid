using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public List<TileSelection> Moves;

    TileSelectionHandler _tileSelectionHandler;
    
    public Move()
    {
        _tileSelectionHandler = new TileSelectionHandler();
    }

    public HashSet<int> GetTiles(int index, Position<Tile> tiles, Position<Entity> entities)
    {
        if (!Util.IsValidOriginAndUnit(index, tiles, entities)) return null;
        HashSet<int> moves = new();
        Vector2Int origin = tiles.GetVector2(index);
        //Unit unit = (Unit)entities.Get(index);
        foreach (TileSelection move in Moves)
        {
            moves.UnionWith(_tileSelectionHandler.GetSelectableTiles(move, origin, tiles, entities));
        }
        moves.Remove(index);
        moves.ExceptWith(entities.GetOccupiedIndices());
        return moves;
    }
}