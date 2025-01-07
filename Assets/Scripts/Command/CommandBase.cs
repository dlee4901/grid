// Command Pattern
using UnityEngine;

public class CommandBase
{
    protected CommandBase() {}

    public virtual void Execute() {}

    public virtual void Execute(GameObject gameObj) {}

    public virtual void Undo() {}

    public virtual void Redo() {}
}