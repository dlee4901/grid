using UnityEngine;

public class DropGameObjectAction : ActionBase
{
    public DropGameObjectAction() {}

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