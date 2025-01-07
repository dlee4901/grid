using UnityEngine;

public class DropGameObjectCommand : CommandBase
{
    public DropGameObjectCommand() {}

    public override void Execute(GameObject gameObj)
    {
        Unit unit = gameObj.GetComponent<Unit>();
        if (unit != null)
        {
            EventManager.Singleton.StartUnitPlaceEvent(unit);
        }
    }

    public override void Undo()
    {

    }

    public override void Redo()
    {
        
    }
}