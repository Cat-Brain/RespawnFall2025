using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BouncerTick : OnTickEffect
{
    public CollisionTick collisionTick;

    [Tooltip("Only bounces colliders that overlap this layermask")]
    public LayerMask bounceMask;
    public float bounceForce;
    [Range(0f, 1f)] public float horizontalMultiplier;
    public float bounceCooldown;
    protected List<(Rigidbody2D rb, float lastBounce)> bounceCooldowns = new();

    public override void OnTick(TickEntity tickEntity)
    {
        bounceCooldowns.RemoveAll(potentialCooldown => Time.time - potentialCooldown.lastBounce >= bounceCooldown);

        if (collisionTick.recentCollider == null ||
            !CMath.LayerOverlapsMask(collisionTick.recentCollider.gameObject.layer, bounceMask) ||
            !collisionTick.recentCollider.TryGetComponent(out Rigidbody2D rb) ||
            bounceCooldowns.Exists(potentialCooldown => potentialCooldown.rb == rb))
        return;

        rb.linearVelocityX *= horizontalMultiplier;
        //rb.linearVelocityY = Mathf.Max(0, rb.linearVelocityY);
        rb.linearVelocityY = bounceForce;

        bounceCooldowns.Add((rb, Time.time));
    }
}
