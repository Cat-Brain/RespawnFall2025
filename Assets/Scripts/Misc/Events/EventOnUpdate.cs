using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EventOnUpdate : MonoBehaviour
{
    public UnityEvent onUpdate;

    public bool active = true;

    public void Update()
    {
        if (active)
            onUpdate?.Invoke();
    }
}
