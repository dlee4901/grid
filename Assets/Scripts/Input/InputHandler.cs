using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputActions;

public enum CommandPreset {DragDrop}
// public enum InputActionMethod {IsPressed, WasPressedThisFrame, WasReleasedThisFrame, WasPerformedThisFrame, WasCompletedThisFrame}

public class InputHandler : IPlayerActions, IUIActions
{
    public bool IsSelectPressed => _playerInputActions.Player.Select.IsPressed();
    //public bool IsSelectPressedThisFrame => _playerInputActions.Player.Select.WasPressedThisFrame();
    public bool IsSelectReleasedThisFrame => _playerInputActions.Player.Select.WasReleasedThisFrame();
    //public bool IsSelectPerformedThisFrame => _playerInputActions.Player.Select.WasPerformedThisFrame();
    //public bool IsSelectCompletedThisFrame => _playerInputActions.Player.Select.WasCompletedThisFrame();

    public bool IsMoveCameraPressed => _playerInputActions.Player.MoveCamera.IsPressed();

    //public bool IsUISelectPressed => _playerInputActions.UI.Select.IsPressed();
    //public bool IsUISelectPressedThisFrame => _playerInputActions.UI.Select.WasPressedThisFrame();
    //public bool IsUISelectReleasedThisFrame => _playerInputActions.UI.Select.WasReleasedThisFrame();
    //public bool IsUISelectPerformedThisFrame => _playerInputActions.UI.Select.WasPerformedThisFrame();
    //public bool IsUISelectCompletedThisFrame => _playerInputActions.UI.Select.WasCompletedThisFrame();

    public event Action SelectPerformed;
    public event Action SelectCanceled;
    public event Action MoveCameraPerformed;
    public event Action<float> ZoomCameraPerformed;

    private PlayerInputActions _playerInputActions;

    public InputHandler()
    {
        if (_playerInputActions == null)
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.AddCallbacks(this);
        }
        
        _playerInputActions.Player.Enable();
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                SelectPerformed?.Invoke();
                break;
            case InputActionPhase.Canceled:
                SelectCanceled?.Invoke();
                break;
        }
    }

    public void OnMoveCamera(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) MoveCameraPerformed?.Invoke();
    }

    public void OnZoomCamera(InputAction.CallbackContext context)
    {
        Debug.Log("OnZoomCamera");
        if (context.phase == InputActionPhase.Performed) ZoomCameraPerformed?.Invoke(context.ReadValue<float>());
    }

    public CommandBase GetCommand(CommandPreset commandPreset)
    {
        if (commandPreset == CommandPreset.DragDrop)
        {
            if (IsSelectPressed) return new DragGameObjectCommand();
            if (IsSelectReleasedThisFrame) return new DropGameObjectCommand();
        }
        return null;
    }

    /*
    protected Dictionary<string, Tuple<InputAction, InputActionMethod, CommandBase>> InputActionMap;

    public InputHandler() 
    {
        InputActionMap = new Dictionary<string, Tuple<InputAction, InputActionMethod, CommandBase>>();
    }

    public InputHandler(InputActionPreset preset) : this()
    {
        if (preset == InputActionPreset.DragDrop)
        {
            AddInputAction("drag gameobject", InputSystem.actions.FindAction("Player/Select"), InputActionMethod.IsPressed, new DragGameObjectCommand());
            AddInputAction("drop gameobject", InputSystem.actions.FindAction("Player/Select"), InputActionMethod.WasReleasedThisFrame, new DropGameObjectCommand());
        }
    }

    public void AddInputAction(string key, InputAction inputAction, InputActionMethod inputActionMethod, CommandBase command)
    {
        InputActionMap.Add(key, new Tuple<InputAction, InputActionMethod, CommandBase>(inputAction, inputActionMethod, command));
    }

    public void DeleteInputAction(string key)
    {
        InputActionMap.Remove(key);
    }

    public CommandBase GetAction(string key)
    {
        return InputActionMap[key].Item3;
    }

    public CommandBase PerformInputAction(InputAction inputAction, InputActionMethod inputActionMethod, CommandBase command)
    {
        switch (inputActionMethod)
        {
            case InputActionMethod.IsPressed:
                if (inputAction.IsPressed()) return command;
                break;
            case InputActionMethod.WasPerformedThisFrame:
                if (inputAction.WasPerformedThisFrame()) return command;
                break;
            case InputActionMethod.WasPressedThisFrame:
                if (inputAction.WasPressedThisFrame()) return command;
                break;
            case InputActionMethod.WasReleasedThisFrame:
                if (inputAction.WasReleasedThisFrame()) return command;
                break;
        }
        return null;
    }

    public CommandBase HandleInput()
    {
        foreach (var (key, value) in InputActionMap)
        {
            CommandBase command = PerformInputAction(value.Item1, value.Item2, value.Item3);
            if (command != null) return command;
        }
        return null;
    }
    */
}