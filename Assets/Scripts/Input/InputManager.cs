using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A wrapper for interacting with Unity's Input System.
/// Example use: InputManager.Instance.Gameplay.Jump.WasPerformedThisFrame()
/// Most common button functions:
///   - WasPerformedThisFrame()
///   - IsPressed()
///   - WasReleasedThisFrame()
/// </summary>
public class InputManager : Singleton<InputManager>
{
    public enum ActionMap
    {
        Gameplay,
        UI
    }
    
    private Controls _controls;

    public Controls.GameplayActions Gameplay => _controls.Gameplay;
    public Controls.UIActions UI => _controls.UI;
    
    public void SetActionMap(ActionMap actionMap)
    {
        DisableAllActionMaps();
        
        switch (actionMap)
        {
            case ActionMap.Gameplay:
                _controls.Gameplay.Enable();
                break;
            case ActionMap.UI:
                _controls.UI.Enable();
                break;
            default:
                Debug.LogError("INPUT_MANAGER: Invalid argument value in attempt to set action map.");
                break;
        }
    }

    private void Start()
    {
        _controls = new();
        SetActionMap(ActionMap.Gameplay);
    }

    private void DisableAllActionMaps()
    {
        foreach (var actionMap in _controls.asset.actionMaps)
        {
            actionMap.Disable();
        }
    }
}
