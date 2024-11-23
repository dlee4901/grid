// Command Pattern
public abstract class ActionBase
{
    protected ActionBase() {}

    public abstract void Execute();

    public abstract void Undo();

    public abstract void Redo();
}