using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EventOnClick : MonoBehaviour
{
    public List<InputActionReference> actions;
    public UnityEvent onPress, onHold, onRelease;

    public bool active;

    public void Awake()
    {
        foreach (InputActionReference action in actions)
        {
            action.action.started += OnPress;
            action.action.performed += OnHold;
            action.action.canceled += OnRelease;
        }
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
