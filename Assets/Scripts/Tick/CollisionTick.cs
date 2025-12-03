using UnityEngine;

public class CollisionTick : MonoBehaviour
{
    public TickEntity tickEntity;

    public bool tickOnDisabled;
    public bool tickOnEnter, tickOnStay, tickOnExit;

    [HideInInspector] public Collider2D recentCollider = null;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if ((!enabled && !tickOnDisabled) || !tickOnEnter)
            return;

        recentCollider = collision.collider;
        tickEntity.Tick();
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if ((!enabled && !tickOnDisabled) || !tickOnStay)
            return;

        recentCollider = collision.collider;
        tickEntity.Tick();
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if ((!enabled && !tickOnDisabled) || !tickOnExit)
            return;

        recentCollider = collision.collider;
        tickEntity.Tick();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if ((!enabled && !tickOnDisabled) || !tickOnEnter)
            return;

        recentCollider = collider;
        tickEntity.Tick();
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if ((!enabled && !tickOnDisabled) || !tickOnStay)
            return;

        recentCollider = collider;
        tickEntity.Tick();
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if ((!enabled && !tickOnDisabled) || !tickOnExit)
            return;

        recentCollider = collider;
        tickEntity.Tick();
    }
}
