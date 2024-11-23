using UnityEngine;
using UnityEngine.InputSystem;

public class UnitInputHandler : InputHandler
{
    int _tileHovered;

    public UnitInputHandler(InputAction selectAction)
    {
        EventManager.Singleton.TileHoverEvent += TileHover;
        SelectAction = selectAction;
        _tileHovered = 0;
    }

    void TileHover(int id)
    {
        _tileHovered = id;
    }

    public ActionBase HandleInput(Unit unit)
    {
        if (SelectAction.IsPressed()) return new HoldUnitAction(unit, GetMousePosition());
        if (SelectAction.WasReleasedThisFrame()) return new PlaceUnitAction(unit, _tileHovered);
        return null;
    }
}