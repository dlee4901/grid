using UnityEngine;

// public enum Direction {N, NE, E, SE, S, SW, W, NW, step, stride, line, diagonal, straight, horizontal, vertical}
// public enum DirectionFacing {N = Direction.N, E = Direction.E, S = Direction.S, W = Direction.W}
// public enum Passthrough {None, Ally, Enemy, All}

[CreateAssetMenu(menuName = "TileSelection")]
public class TileSelection : ScriptableObject
{
    public Direction Direction;
    public Passthrough Passthrough;
    public int Origin;
    public int Distance;
    public int Cost;
    public bool Exact;
    public bool RelativeFacing;
    public UnitMove Chain;
}
