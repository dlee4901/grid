using UnityEngine;

[CreateAssetMenu(menuName = "TargetProjectile")]
public class Projectile : TargetBase
{
    [Header("Projectile")]
    public int Distance;
    public int Width;
    public DirectionCardinal Direction;
}