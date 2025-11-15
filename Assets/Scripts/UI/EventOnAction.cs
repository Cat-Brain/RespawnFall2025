using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EventOnAction : MonoBehaviour
{
    public InputActionReference action;
    public UnityEvent onPress, onHold, onRelease;

    public bool active;

    public void Awake()
    {
        action.action.started += OnPress;
        action.action.performed += OnHold;
        action.action.canceled += OnRelease;
    }

    public void OnPress(InputAction.CallbackContext callbackContext)
    {
        if (active)
            onPress?.Invoke();
    }

    public void OnHold(InputAction.CallbackContext callbackContext)
    {
        if (active)
            onHold?.Invoke();
    }

    public void OnRelease(InputAction.CallbackContext callbackContext)
    {
        if (active)
            onRelease?.Invoke();
    }
}
