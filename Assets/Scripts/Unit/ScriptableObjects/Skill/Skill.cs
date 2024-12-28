public enum Effect {Position, Health, Shield, Damage, Counter}
public enum Buff {}

public class Skill
{
    public int Prep;
    public int Cooldown;

    public TargetBase Target;

    public Effect Effect;
    public int Delta;
}