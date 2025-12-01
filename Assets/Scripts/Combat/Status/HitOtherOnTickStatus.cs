using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HitOtherOnTickStatus", menuName = "StatusComponents/HitOtherOnTickStatus")]
public class HitOtherOnTickStatus : StatusComponent
{
    public int tickIndex;
    public Hit hit;
    public float hitRadius;
    public LayerMask hitMask;

    public override void Tick(StatusEffect effect, int tickIndex)
    {
        if (this.tickIndex != tickIndex)
            return;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
            effect.health.transform.position, hitRadius, hitMask);

        foreach (Collider2D col in hitColliders)
            if (col.TryGetComponent(out HealthInst health))
                health.ApplyHit(hit);
    }
}