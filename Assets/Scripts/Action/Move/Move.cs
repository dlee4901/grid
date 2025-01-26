using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private List<TileSelection> _moves;

    TileSelectionHandler _tileSelectionHandler;
    
    public Move()
    {
        _tileSelectionHandler = new TileSelectionHandler();
        //if (_moves == null) _moves = moves;
    }

    public HashSet<int> GetTiles(int index, Position<Tile> tiles, Position<Entity> entities)
    {
        if (!Util.IsValidOriginAndUnit(index, tiles, entities)) return null;
        HashSet<int> moves = new();
        Vector2Int origin = tiles.GetVector2(index);
        //Unit unit = (Unit)entities.Get(index);
        foreach (TileSelection move in _moves)
        {
            moves.UnionWith(_tileSelectionHandler.GetSelectableTiles(move, origin, tiles, entities));
        }
        moves.Remove(index);
        moves.ExceptWith(entities.GetOccupiedIndices());
        return moves;
    }
}