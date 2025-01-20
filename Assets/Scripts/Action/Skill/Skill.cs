using UnityEngine;

public enum Effect {Position, Health, Shield, Damage, Counter}
public enum Buff {}

public class Skill : MonoBehaviour
{
    public int PrepTurns;
    public int Cooldown;
    public int Cost;

    public TargetBase Target;

    public Effect Effect;
    public int Delta;
}