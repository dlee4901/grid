using UnityEngine;

public class TargetBase : ScriptableObject
{
    [Header("Target")]
    public Team Team;
    public Entity Entity;
    public int Iterations;
}