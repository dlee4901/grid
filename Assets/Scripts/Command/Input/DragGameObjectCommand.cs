using UnityEngine;

public class DragGameObjectCommand : CommandBase
{
    public DragGameObjectCommand() {}

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