using UnityEngine;

public class HoldUnitAction : ActionBase
{
    Unit _unit;
    Vector3 _mousePosition;

    public HoldUnitAction(Unit unit, Vector3 mousePosition) : base()
    {
        _unit = unit;
        _mousePosition = mousePosition;
    }

    public override void Execute()
    {
        _unit.HoldUnit(_mousePosition);
    }

    public override void Undo()
    {

    }

    public override void Redo()
    {
        
    }
}