using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler
{
    protected InputAction SelectAction;
    protected ActionBase Action;

    public InputHandler() {}

    public ActionBase HandleInput()
    {
        if (SelectAction.IsPressed()) return Action;
        return null;
    }

    public Vector3 GetMousePosition(bool zeroed=true)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (zeroed) mousePosition.z = 0f;
        return mousePosition;
    }
}