using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandler
{
    public TargetHandler() {}

    public HashSet<int> GetTargets(TargetBase target, int origin, Position<Tile> tiles, Position<Unit> units)
    {
        HashSet<int> targets = new();
        Type type = target.GetType();
        Vector2Int origin_ = tiles.GetVector2(origin);
        if (type == typeof(Individual))      return GetTargetsIndividual(target, origin_, tiles, units);
        else if (type == typeof(AOE))        return GetTargetsAOE(target, origin_, tiles, units);
        else if (type == typeof(Projectile)) return GetTargetsProjectile(target, origin_, tiles, units);
        else if (type == typeof(Beam))       return GetTargetsBeam(target, origin_, tiles, units);
        return targets;
    }

    public HashSet<int> GetTargetsTeamEntity(Team team, Vector2Int origin, Position<Unit> units)
    {
        HashSet<int> occupiedIndices = units.GetOccupiedIndices();
        foreach (int index in occupiedIndices)
        {
            Unit unit = units.Get(index);
            //if (unit.Stats.PlayerController)
        }
        return occupiedIndices;
    }

    public HashSet<int> GetTargetsIndividual(TargetBase target, Vector2Int origin, Position<Tile> tiles, Position<Unit> units)
    {
        HashSet<int> skillTargets = new();
        Individual targetIndividual = (Individual)target;
        TileSelection selectionArea = targetIndividual.SelectionArea;
        TileSelectionHandler tileSelectionHandler = new();
        HashSet<int> selectableTiles = tileSelectionHandler.GetSelectableTiles(selectionArea, origin, tiles, units);
        HashSet<int> occupiedIndices = units.GetOccupiedIndices();
        //var test = "test";
        //var nameTest = nameof(test);
        
        return skillTargets;
    }

    public HashSet<int> GetTargetsAOE(TargetBase target, Vector2Int origin, Position<Tile> tiles, Position<Unit> units)
    {
        HashSet<int> skillTargets = new();
        return skillTargets;
    }

    public HashSet<int> GetTargetsProjectile(TargetBase target, Vector2Int origin, Position<Tile> tiles, Position<Unit> units)
    {
        HashSet<int> skillTargets = new();
        return skillTargets;
    }

    public HashSet<int> GetTargetsBeam(TargetBase target, Vector2Int origin, Position<Tile> tiles, Position<Unit> units)
    {
        HashSet<int> skillTargets = new();
        return skillTargets;
    }
}