using DG.Tweening;
using UnityEngine;

public class GhostFlower : Enemy
{
    public EntityStat damageStat;
    public Projectile projectile;
    public GameObject projectilePrefab;
    public Transform projectileIndicator;

    public float projectileFireTime, projectileWaitTime;

    public float projectileWaitTimer;
    public bool shooting = false;

    public new void Awake()
    {
        damageStat.baseValue = projectile.hit.damage;
        base.Awake();
    }

    public override void ActiveUpdate()
    {
        if (shooting)
            return;

        base.ActiveUpdate();
        if (!playerVisible || state != AIState.ACTIVE)
        {
            projectileWaitTimer = projectileWaitTime;
            return;
        }

        projectileWaitTimer = Mathf.Max(0, projectileWaitTimer - Time.deltaTime);
        if (projectileWaitTimer > 0)
            return;

        shooting = true;
        projectileIndicator.DOScale(projectile.radius, projectileFireTime).OnComplete(() =>
        {
            projectileIndicator.localScale = Vector3.zero;
            shooting = false;
            projectileWaitTimer = projectileWaitTime;

            ProjectileInst projectileInst =
                Instantiate(projectilePrefab, projectileIndicator.position, transform.rotation).
                GetComponent<ProjectileInst>();
            projectileInst.Init(projectile, -transform.up);
            projectileInst.data.hit.damage = damageStat.value;
        });
    }

    public override void ActiveEnter()
    {
        projectileWaitTimer = projectileWaitTime;
        base.ActiveEnter();
    }
}
