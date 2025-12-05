using UnityEngine;
using UnityEngine.InputSystem;

public class OneWayDropDown : MonoBehaviour
{
    public InputActionReference heldAction;
    public LayerMask heldMask, unheldMask;
    public Collider2D col;

    void Update()
    {
        col.excludeLayers = heldAction.action.inProgress ? heldMask : unheldMask;
    }
}
