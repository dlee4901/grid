// Command Pattern
using UnityEngine;

public class ActionBase
{
    protected ActionBase() {}

    public virtual void Execute() {}

    public virtual void Execute(GameObject gameObj) {}

    public virtual void Undo() {}

    public virtual void Redo() {}
}