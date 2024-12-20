using System;
using UnityEngine;

// public enum Direction {None, N, NE, E, SE, S, SW, W, NW, step, stride, line, diagonal, straight, horizontal, vertical}
// public enum DirectionFacing {None, N = Direction.N, E = Direction.E, S = Direction.S, W = Direction.W}
[Flags]
public enum Team {Neutral=1, Ally=2, Enemy=4}
[Flags]
public enum Entity {Unit=1, Obstacle=2}

[CreateAssetMenu(menuName = "TileSelection")]
public class TileSelection : ScriptableObject
{
    public Direction Direction;
    public Team Passthrough;
    public int Distance;
    public bool Exact;
    public bool RelativeFacing;
    public TileSelection Chain;
}

// enum TargetType {Select, Projectile, Beam, AOE}

// Select
// Direction: any
// Passthrough: 
