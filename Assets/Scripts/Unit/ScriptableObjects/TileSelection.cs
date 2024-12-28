using System;
using UnityEngine;

public enum Direction {N, NE, E, SE, S, SW, W, NW, step, stride, line, diagonal, straight, horizontal, vertical}
public enum DirectionCardinal {N, NE, E, SE, S, SW, W, NW}
public enum DirectionFacing {N, E, S, W}
[Flags] public enum Team {Neutral=1, Ally=2, Enemy=4}
[Flags] public enum Entity {Unit=1, Obstacle=2}

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
