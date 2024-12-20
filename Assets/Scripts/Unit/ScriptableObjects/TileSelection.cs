using UnityEngine;

// public enum Direction {N, NE, E, SE, S, SW, W, NW, step, stride, line, diagonal, straight, horizontal, vertical}
// public enum DirectionFacing {N = Direction.N, E = Direction.E, S = Direction.S, W = Direction.W}
public enum Team {None, Neutral, Ally, Enemy}
public enum Entity {None, Unit, Structure}

[CreateAssetMenu(menuName = "TileSelection")]
public class TileSelection : ScriptableObject
{
    public Direction Direction;
    public EnumBinaryString<Team> Passthrough = new EnumBinaryString<Team>("100");
    public int Origin;
    public int Distance;
    public int Cost;
    public bool Exact;
    public bool RelativeFacing;
    public TileSelection Chain;
}

// enum TargetType {Select, Projectile, Beam, AOE}

// Select
// Direction: any
// Passthrough: 
