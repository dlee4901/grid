public class PlaceUnitAction : ActionBase
{
    Unit _unit;
    int _positionIdx;

    public PlaceUnitAction(Unit unit, int positionIdx) : base()
    {
        _unit = unit;
        _positionIdx = positionIdx;
    }

    public override void Execute()
    {
        EventManager.Singleton.StartUnitPlaceEvent(_unit);
    }

    public override void Undo()
    {

    }

    public override void Redo()
    {
        
    }
}