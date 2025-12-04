using UnityEngine;

[CreateAssetMenu(fileName = "New HitOtherOnTickStatus", menuName = "StatusComponents/HitOtherOnTickStatus")]
public class HitOtherOnTickStatus : StatusComponent
{
    public int tickIndex;
    public Hit hit;
    public float hitRadius;
    public LayerMask hitMask;
    public bool hitSelf;

    public override void Tick(StatusEffect effect, int tickIndex)
    {
        if (this.tickIndex != tickIndex)
            return;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
            effect.health.transform.position, hitRadius, hitMask);

        foreach (Collider2D col in hitColliders)
            if ((hitSelf || col.transform != effect.health.transform)
                && col.TryGetComponent(out HealthInst health))
                health.ApplyHit(hit);
    }
}