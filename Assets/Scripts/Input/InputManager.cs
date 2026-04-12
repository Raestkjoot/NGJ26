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
    
    public enum InputType
    {
        MouseAndKeyboard,
        Gamepad
    }
    
    private Controls _controls;
    private InputType _currentInputType;
    private Vector2 _lastRegisteredMousePosition;

    private const float MouseMoveThreshold = 1.0f;
    
    public Controls.UIActions UI => _controls.UI;

    public bool GetIsYoinking()
    {
        if (_controls.Gameplay.Yoink.activeControl != null)
        {
            SetInputDeviceType(_controls.Gameplay.Yoink.activeControl.device.name.Equals("Mouse")
                ? InputType.MouseAndKeyboard
                : InputType.Gamepad);
        }
        
        return _controls.Gameplay.Yoink.WasPerformedThisFrame();
    }

    public bool GetWasEscPressed()
    {
        return _controls.Gameplay.Escape.WasPerformedThisFrame();
    }
    
    public Vector2 GetMoveDirection()
    {
        _controls.Gameplay.Move.ReadValue<Vector2>();

        Vector2 moveDirection = _controls.Gameplay.MoveGamepad.ReadValue<Vector2>();

        // If we get input from a gamepad that goes past deadzone, we switch to gamepad controls
        if (moveDirection != Vector2.zero)
        {
            SetInputDeviceType(InputType.Gamepad);
            return moveDirection;
        }
        
        Vector2 mousePosition = _controls.Gameplay.Move.ReadValue<Vector2>();

        // If we are using gamepad controls, we want to move the mouse past a threshold before we switch back to mouse controls
        if (_currentInputType == InputType.Gamepad)
        {
            float mouseDelta = (_lastRegisteredMousePosition - mousePosition).magnitude;
            if (mouseDelta < MouseMoveThreshold)
            {
                return moveDirection;
            }
        }

        SetInputDeviceType(InputType.MouseAndKeyboard);

        Vector2 screenCenter = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        moveDirection = (mousePosition - screenCenter).normalized;
        _lastRegisteredMousePosition = mousePosition;
        
        return moveDirection;
    }

    public void SetCursorVisibility(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
    
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

    protected override void Awake()
    {
        base.Awake();
        _controls = new();
        SetActionMap(ActionMap.Gameplay);
    }

    private void SetInputDeviceType(InputType inputType)
    {
        _currentInputType = inputType;
        SetCursorVisibility(inputType == InputType.MouseAndKeyboard);
        // TODO: make an event for when device changes. Could be useful for UI changes :eyes:
    }

    private void OnDisable()
    {
        DisableAllActionMaps();
    }

    private void DisableAllActionMaps()
    {
        foreach (var actionMap in _controls.asset.actionMaps)
        {
            actionMap.Disable();
        }
    }
}
