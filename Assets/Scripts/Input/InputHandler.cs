using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputActionPreset {DragDrop}
public enum InputActionMethod {IsPressed, WasPerformedThisFrame, WasPressedThisFrame, WasReleasedThisFrame}

public class InputHandler
{
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
}