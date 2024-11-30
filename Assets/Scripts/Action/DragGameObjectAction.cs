using UnityEngine;

public class DragGameObjectAction : ActionBase
{
    public DragGameObjectAction() {}

    public override void Execute(GameObject gameObj)
    {
        gameObj.transform.localPosition = Util.GetMousePosition();
    }

    public override void Undo()
    {

    }

    public override void Redo()
    {
        
    }
}