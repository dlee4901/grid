using UnityEngine;

[CreateAssetMenu(menuName = "TargetBeam")]
public class Beam : TargetBase
{
    [Header("Beam")]
    public int Distance;
    public int Width;
    public int DamageFalloff;
    public DirectionCardinal Direction;
}