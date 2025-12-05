using UnityEngine;
using UnityEngine.InputSystem;

public class TickWhenHoveredEntity : TickEntity
{
    public Collider2D col;

    public override void Tick()
    {
        if (col.OverlapPoint(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue())))
            base.Tick();
    }

    public virtual void BypassTick()
    {
        base.Tick();
    }
}
