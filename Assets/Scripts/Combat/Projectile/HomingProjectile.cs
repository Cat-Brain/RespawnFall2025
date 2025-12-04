using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "New HomingProjectile", menuName = "Projectiles/HomingProjectile")]
public class HomingProjectile : Projectile
{
    public float searchRadius, rotationSpeed;

    public override void OnUpdate()
    {
        Collider2D[] nearbyTargets = Physics2D.OverlapCircleAll(inst.transform.position, searchRadius, targetMask);

        if (nearbyTargets.Length <= 0)
            return;

        float bestDist = Mathf.Infinity;
        Collider2D nearestCollider = null;
        foreach (Collider2D target in nearbyTargets)
        {
            float newDist = target.Distance(inst.col).distance;
            if (newDist < bestDist)
                nearestCollider = target;
        }
        Vector2 desiredDir = (nearestCollider.transform.position - inst.transform.position).normalized;

        float rotation = Vector2.SignedAngle(desiredDir, inst.direction);

        inst.direction = CMath.Rotate(inst.direction,
            CMath.TrySub(rotation, rotationSpeed * Time.deltaTime) - rotation);
    }
}
