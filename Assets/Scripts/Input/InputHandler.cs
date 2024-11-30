using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputActionMethod {IsPressed, WasPerformedThisFrame, WasPressedThisFrame, WasReleasedThisFrame}

public class InputHandler
{
    protected Dictionary<string, Tuple<InputAction, InputActionMethod, ActionBase>> InputActionMap;

    public InputHandler() 
    {
        InputActionMap = new Dictionary<string, Tuple<InputAction, InputActionMethod, ActionBase>>();
    }

    public void AddInputAction(string key, InputAction inputAction, InputActionMethod inputActionMethod, ActionBase action)
    {
        InputActionMap.Add(key, new Tuple<InputAction, InputActionMethod, ActionBase>(inputAction, inputActionMethod, action));
    }

    public void DeleteInputAction(string key)
    {
        InputActionMap.Remove(key);
    }

    public ActionBase GetAction(string key)
    {
        return InputActionMap[key].Item3;
    }

    public ActionBase PerformInputAction(InputAction inputAction, InputActionMethod inputActionMethod, ActionBase action)
    {
        switch (inputActionMethod)
        {
            case InputActionMethod.IsPressed:
                if (inputAction.IsPressed()) return action;
                break;
            case InputActionMethod.WasPerformedThisFrame:
                if (inputAction.WasPerformedThisFrame()) return action;
                break;
            case InputActionMethod.WasPressedThisFrame:
                if (inputAction.WasPressedThisFrame()) return action;
                break;
            case InputActionMethod.WasReleasedThisFrame:
                if (inputAction.WasReleasedThisFrame()) return action;
                break;
        }
        return null;
    }

    public ActionBase HandleInput()
    {
        foreach (var (key, value) in InputActionMap)
        {
            ActionBase action = PerformInputAction(value.Item1, value.Item2, value.Item3);
            Debug.Log(action);
            if (action != null) return action;
        }
        return null;
    }
}