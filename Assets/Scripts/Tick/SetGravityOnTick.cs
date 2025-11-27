using UnityEngine;

public class SetGravityOnTick : OnTickEffect
{
    public float gravity;

    public override void OnTick(TickEntity tickEntity)
    {
        GetComponent<Rigidbody2D>().gravityScale = gravity;
    }
}
