using DG.Tweening;
using UnityEngine;
using static Charger;

public class GBCBoss : Enemy
{
    public GameObject smokePrefab, projectilePrefab;
    public Enemy summon;
    public Projectile projectile;

    public HealthInst health;
    public Rigidbody2D rb;
    public Collider2D col;
    public Transform headTransform, endTransform;

    public float submergeTime, indicateTime, digTime, emergeTime;

    public float chargeAccel, chargeSpeed, chargeRadius;
    public LayerMask normalOverlapMask, hitPlayerOverlapMask, chargeFriendlyFireMask, chargePlayerMask;
    public Hit friendlyFireHit, playerHit;

    public float shootTime, summonTime,
        phase1OuterShootTime, phase1InnerShootTime, phase1SummonTime,
        chargeShootTime, phase2OuterShootTime, phase2InnerShootTime, phase2SummonTime;

    public enum Phase
    {
        PHASE_1, PHASE_2
    }
    public Phase phase = Phase.PHASE_1;

    public enum Attack
    {
        CHARGE_RIGHT, CHARGE_LEFT, SHOOT_FROM_LEFT, SHOOT_FROM_RIGHT, SHOOT_FROM_MIDDLE, SUMMON,
        ATTACK_COUNT, NULL_ATTACK
    }
    public Attack attack;

    public enum DigState
    {
        SUBMERGING, DIGGING, INDICATING, EMERGING, ATTACKING
    }
    public DigState digState;

    public float digStateTimer, shootTimer;
    public Vector2[] bossPoints;
    public bool hasChargedPlayer = false;

    public void Start()
    {
        enemyManager.bossPoints.Sort((p1, p2) => p1.transform.position.x.CompareTo(p2.transform.position.x));
        bossPoints = enemyManager.bossPoints.ConvertAll<Vector2>((point) => point.transform.position).ToArray();
    }

    public override void ActiveEnter()
    {
        digState = DigState.ATTACKING;
        digStateTimer = 0;
        attack = Attack.NULL_ATTACK;
        base.ActiveEnter();
    }

    public override void ActiveUpdate()
    {
        phase = health.health > health.data.maxHealth * 0.5f ? Phase.PHASE_1 : Phase.PHASE_2;

        digStateTimer = Mathf.Max(0, digStateTimer - Time.deltaTime);
        switch (digState)
        {
            case DigState.SUBMERGING:
                if (digStateTimer > 0)
                    break;

                digState = DigState.DIGGING;
                digStateTimer = digTime;

                col.excludeLayers = normalOverlapMask;
                rb.bodyType = RigidbodyType2D.Static;
                rb.simulated = false;
                col.enabled = false;

                break;
            case DigState.DIGGING:
                if (digStateTimer > 0)
                    break;

                digState = DigState.INDICATING;
                digStateTimer = indicateTime;

                attack = (Attack)Random.Range(0, (int)Attack.ATTACK_COUNT);
                switch (attack)
                {
                    case Attack.CHARGE_RIGHT:
                        SpawnSmoke(bossPoints[0], (int)spawnDimensions.x);
                        break;
                    case Attack.CHARGE_LEFT:
                        SpawnSmoke(bossPoints[2], (int)spawnDimensions.x);
                        break;
                    case Attack.SHOOT_FROM_RIGHT:
                        SpawnSmoke(bossPoints[2], (int)spawnDimensions.x);
                        break;
                    case Attack.SHOOT_FROM_LEFT:
                        SpawnSmoke(bossPoints[0], (int)spawnDimensions.x);
                        break;
                    case Attack.SHOOT_FROM_MIDDLE:
                        SpawnSmoke(bossPoints[1], (int)spawnDimensions.y);
                        break;
                    case Attack.SUMMON:
                        SpawnSmoke(bossPoints[1], (int)spawnDimensions.y);
                        break;
                }
                break;
            case DigState.INDICATING:
                if (digStateTimer > 0)
                    break;

                digState = DigState.EMERGING;
                digStateTimer = emergeTime;

                switch (attack)
                {
                    case Attack.CHARGE_RIGHT:
                        transform.localScale = Vector3.one;
                        HorizontalEmergeAtPoint(bossPoints[0]);
                        break;
                    case Attack.CHARGE_LEFT:
                        transform.localScale = CMath.V3NPP(Vector3.one);
                        HorizontalEmergeAtPoint(bossPoints[2]);
                        break;
                    case Attack.SHOOT_FROM_RIGHT:
                        transform.localScale = CMath.V3NPP(Vector3.one);
                        HorizontalEmergeAtPoint(bossPoints[2]);
                        break;
                    case Attack.SHOOT_FROM_LEFT:
                        transform.localScale = Vector3.one;
                        HorizontalEmergeAtPoint(bossPoints[0]);
                        break;
                    case Attack.SHOOT_FROM_MIDDLE:
                        VerticalEmergeAtPoint(bossPoints[1]);
                        break;
                    case Attack.SUMMON:
                        VerticalEmergeAtPoint(bossPoints[1]);
                        break;
                }
                break;
            case DigState.EMERGING:
                if (digStateTimer > 0)
                    break;

                digState = DigState.ATTACKING;

                shootTimer = 0;
                switch (attack)
                {
                    case Attack.SHOOT_FROM_RIGHT:
                    case Attack.SHOOT_FROM_LEFT:
                    case Attack.SHOOT_FROM_MIDDLE:
                        digStateTimer = shootTime;
                        break;
                    case Attack.SUMMON:
                        digStateTimer = summonTime;
                        break;
                }

                break;
            case DigState.ATTACKING:
                shootTimer -= Time.deltaTime;

                switch (attack)
                {
                    case Attack.CHARGE_RIGHT:
                        rb.linearVelocityX = CMath.TryAdd(rb.linearVelocityX,
                            Time.deltaTime * chargeAccel, chargeSpeed);

                        if (phase == Phase.PHASE_2 && shootTimer < 0)
                        {
                            ProjectileInst projectileInst =
                                Instantiate(projectilePrefab, endTransform.position, transform.rotation).
                                GetComponent<ProjectileInst>();
                            projectileInst.Init(projectile, -transform.right);
                            shootTimer = chargeShootTime;
                        }

                        TryChargeDamage();

                        if (transform.position.x < bossPoints[2].x - 1)
                            break;

                        digState = DigState.SUBMERGING;
                        digStateTimer = submergeTime;

                        HorizontalSubmerge();
                        hasChargedPlayer = false;

                        break;
                    case Attack.CHARGE_LEFT:
                        rb.linearVelocityX = CMath.TryAdd(rb.linearVelocityX,
                            -Time.deltaTime * chargeAccel, chargeSpeed);

                        if (phase == Phase.PHASE_2 && shootTimer < 0)
                        {
                            ProjectileInst projectileInst =
                                Instantiate(projectilePrefab, endTransform.position, transform.rotation).
                                GetComponent<ProjectileInst>();
                            projectileInst.Init(projectile, -transform.right);
                            shootTimer = chargeShootTime;
                        }

                        TryChargeDamage();

                        if (transform.position.x > bossPoints[0].x + 1)
                            break;

                        digState = DigState.SUBMERGING;
                        digStateTimer = submergeTime;

                        HorizontalSubmerge();
                        hasChargedPlayer = false;

                        break;
                    case Attack.SHOOT_FROM_RIGHT:
                    case Attack.SHOOT_FROM_LEFT:
                        if (shootTimer < 0)
                        {
                            ProjectileInst projectileInst =
                                Instantiate(projectilePrefab, headTransform.position, transform.rotation).
                                GetComponent<ProjectileInst>();
                            projectileInst.Init(projectile,
                                (trackedPlayer.position - headTransform.position).normalized);
                            shootTimer = phase == Phase.PHASE_1 ? phase1OuterShootTime : phase2OuterShootTime;
                        }

                        if (digStateTimer > 0)
                            break;

                        digState = DigState.SUBMERGING;
                        digStateTimer = submergeTime;

                        HorizontalSubmerge();

                        break;
                    case Attack.SHOOT_FROM_MIDDLE:
                        if (shootTimer < 0)
                        {
                            ProjectileInst projectileInst =
                                Instantiate(projectilePrefab, headTransform.position, transform.rotation).
                                GetComponent<ProjectileInst>();
                            projectileInst.Init(projectile,
                                (trackedPlayer.position - headTransform.position).normalized);
                            shootTimer = phase == Phase.PHASE_1 ? phase1InnerShootTime : phase2InnerShootTime;
                        }

                        if (digStateTimer > 0)
                            break;

                        digState = DigState.SUBMERGING;
                        digStateTimer = submergeTime;

                        VerticalSubmerge();

                        break;
                    case Attack.SUMMON:
                        if (shootTimer < 0)
                        {
                            enemyManager.SpawnEnemy(summon);
                            shootTimer = phase == Phase.PHASE_1 ? phase1SummonTime : phase2SummonTime;
                        }

                        if (digStateTimer > 0)
                            break;

                        digState = DigState.SUBMERGING;
                        digStateTimer = submergeTime;

                        VerticalSubmerge();

                        break;
                    default:
                        if (digStateTimer > 0)
                            break;

                        digState = DigState.SUBMERGING;
                        digStateTimer = submergeTime;

                        HorizontalSubmerge();
                        break;
                }

                break;
        }
    }

    public void HorizontalEmergeAtPoint(Vector2 point)
    {
        transform.SetPositionAndRotation(point, Quaternion.identity);
        transform.DOScaleY(1, emergeTime).From(0.01f);
        rb.simulated = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        col.enabled = true;
    }

    public void HorizontalSubmerge()
    {
        transform.DOScaleY(0.01f, submergeTime);
        SpawnSmokeBeneath();
    }

    public void VerticalEmergeAtPoint(Vector2 point)
    {
        transform.localScale = Random.Range(0, 2) == 0 ? Vector3.one : CMath.V3PNP(Vector3.one);
        transform.rotation = Quaternion.Euler(0, 0, 90);
        transform.DOMove(point, emergeTime).
            From(point - spawnDimensions.x * 0.5f * Vector2.up);
        rb.simulated = true;
        rb.bodyType = RigidbodyType2D.Static;
        col.enabled = true;
    }

    public void VerticalSubmerge()
    {
        transform.DOMove((Vector2)transform.position - (spawnDimensions.x * 0.5f + 1) * Vector2.up, submergeTime);
        SpawnSmoke(bossPoints[1], (int)spawnDimensions.y);
    }

    public void SpawnSmoke(Vector2 position, int count)
    {
        position.x -= count * 0.5f - 0.5f;
        for (int i = 0; i < count; i++, position.x++)
            Instantiate(smokePrefab, position, Quaternion.identity);
    }

    public void SpawnSmokeBeneath()
    {
        SpawnSmoke((Vector2)transform.position - (spawnDimensions.y * 0.5f - 0.5f) * Vector2.up, (int)spawnDimensions.x);
    }

    public void TryChargeDamage()
    {
        int tempLayer = gameObject.layer;
        gameObject.layer = 2;

        Collider2D[] friendlyFireOverlaps = Physics2D.OverlapCircleAll(
            headTransform.position, chargeRadius, chargeFriendlyFireMask);
        
        gameObject.layer = tempLayer;

        foreach (Collider2D overlap in friendlyFireOverlaps)
            if (overlap.transform != transform && overlap.TryGetComponent(out HealthInst health))
                health.ApplyHit(friendlyFireHit);

        if (hasChargedPlayer)
            return;

        gameObject.layer = 2;

        Collider2D[] playerOverlaps = Physics2D.OverlapCircleAll(
            headTransform.position, chargeRadius, chargePlayerMask);

        gameObject.layer = tempLayer;

        foreach (Collider2D overlap in playerOverlaps)
            if (overlap.transform != transform && overlap.TryGetComponent(out HealthInst health))
            {
                health.ApplyHit(playerHit);
                col.excludeLayers = hitPlayerOverlapMask;
                hasChargedPlayer = true;
            }
    }

    public new void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(headTransform.position, chargeRadius);
    }
}
