using UnityEngine;

public enum Direction {N, NE, E, SE, S, SW, W, NW, step, stride, line, diagonal, straight, horizontal, vertical}
public enum DirectionFacing {N = Direction.N, E = Direction.E, S = Direction.S, W = Direction.W}
public enum Passthrough {None, Ally, Enemy, All}

[CreateAssetMenu(menuName = "Unit Move")]
public class UnitMove : ScriptableObject
{
    public Direction Direction;
    public int Distance;
    public int Cost;
    public Passthrough Passthrough;
    public bool Exact;
    public bool RelativeFacing;
    public UnitMove Chain;

    // public void ResetMovement()
    // {
    //     direction = Direction.step;
    //     distance = 0;
    //     exact = false;
    //     relativeFacing = false;
    //     chain = null;
    // }

    // void Init(Direction _direction, int _distance, bool _exact, bool _relativeFacing, UnitMovement _chain=null)
    // {
    //     direction = _direction;
    //     distance = _distance;
    //     exact = _exact;
    //     relativeFacing = _relativeFacing;
    //     chain = _chain;
    // }

    // public static UnitMovement Create(Direction _direction, int _distance, bool _exact, bool _relativeFacing, UnitMovement _chain=null)
    // {
    //     var unitMovement = ScriptableObject.CreateInstance<UnitMovement>();
    //     unitMovement.Init(_direction, _distance, _exact, _relativeFacing, _chain);
    //     return unitMovement;
    // }
}