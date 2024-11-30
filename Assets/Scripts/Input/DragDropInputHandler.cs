using UnityEngine;
using UnityEngine.InputSystem;

public class DragDropInputHandler : InputHandler
{
    public DragDropInputHandler()
    {
        AddInputAction("drag gameobject", InputSystem.actions.FindAction("Player/Select"), InputActionMethod.IsPressed, new DragGameObjectAction());
        AddInputAction("drop gameobject", InputSystem.actions.FindAction("Player/Select"), InputActionMethod.WasReleasedThisFrame, new DropGameObjectAction());
    }
}