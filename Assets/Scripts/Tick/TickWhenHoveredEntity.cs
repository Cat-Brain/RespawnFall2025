using UnityEngine;
using UnityEngine.InputSystem;

public class TickWhenHoveredEntity : TickEntity
{
    public Collider2D col;

    public override void Tick()
    {
        Debug.Log(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        if (col.OverlapPoint(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue())))
        {
            Debug.Log("?");
            base.Tick();
        }
    }
}
