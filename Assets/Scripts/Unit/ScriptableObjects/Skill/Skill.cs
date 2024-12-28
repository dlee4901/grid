public enum Effect {ChangePosition, ChangeHealth, ChangeShield, ChangeDamage, ChangeCounter, }
public enum Buff {}

public class Skill
{
    public int Prep;
    public int Cooldown;

    public TargetBase Target;
}