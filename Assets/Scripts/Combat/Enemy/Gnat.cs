using DG.Tweening;
using UnityEngine;

public class Gnat : Enemy
{
    [HideInInspector] public Rigidbody2D rb;

    public Projectile projectile;
    public GameObject projectilePrefab;
    public Transform leftWing, rightWing, projectileIndicator;
    public EntityStat speedStat, damageStat;

    public float projectileFireTime, projectileStunTime, projectileWaitTime, projectileRecoil;
    public float minWingRot, maxWingRot, wingRotFrequency;
    public float normalGravity, stunnedGravity;

    public LayerMask avoidanceMask;
    public float avoidanceDist, avoidanceAccel, avoidanceSpeed;

    public float minIdleMoveTime, maxIdleMoveTime,
        minIdleMoveSpeed, maxIdleMoveSpeed, idleMoveAccel;

    public float chaseSpringFrequency, chaseSpringDamping, chaseDistance;

    public float rotateSpringFrequency, rotateSpringDamping;

    public PhysicsMaterial2D normalPhyMat, stunnedPhyMat;

    public Vector2 desiredDir;

    public bool shooting;
    public float projectileWaitTimer;
    public float idleMoveTimer, idleMoveSpeed;
    public Vector2 idleMoveDir;
    public bool idleMoving; // True means Moving, false means still

    public float wingRotTimer;
    public SpringUtils.tDampedSpringMotionParams chaseSpring = new(), rotateSpring = new();

    public void RandomizeIdleMove()
    {
        idleMoveTimer = Random.Range(minIdleMoveTime, maxIdleMoveTime);
        if (!idleMoving)
            return;
        idleMoveSpeed = Random.Range(minIdleMoveSpeed, maxIdleMoveSpeed);
        idleMoveDir = CMath.Rotate(Vector2.right, Random.value * Mathf.PI * 2);
    }

    public new void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        base.Awake();

        desiredDir = Vector2.down;
        idleMoving = false;
        RandomizeIdleMove();

        damageStat.baseValue = projectile.hit.damage;
    }

    public void LateUpdate()
    {
        if (state == AIState.STUNNED)
        {
            rb.gravityScale = stunnedGravity;
            return;
        }

        rb.gravityScale = normalGravity;

        wingRotTimer += Time.deltaTime * wingRotFrequency;
        float wingRot = Mathf.Lerp(minWingRot, maxWingRot, 0.5f + 0.5f * Mathf.Sin(wingRotTimer * Mathf.PI));
        leftWing.transform.localRotation = Quaternion.Euler(0, 0, wingRot);
        rightWing.transform.localRotation = Quaternion.Euler(0, 0, -wingRot);
    }

    public void FixedUpdate()
    {
        rb.sharedMaterial = state == AIState.STUNNED ? stunnedPhyMat : normalPhyMat;

        float rotation = -Vector2.SignedAngle(-transform.up, desiredDir), velocity = rb.angularVelocity;

        SpringUtils.CalcDampedSpringMotionParams(ref rotateSpring, Time.deltaTime, rotateSpringFrequency, rotateSpringDamping);
        SpringUtils.UpdateDampedSpringMotion(ref rotation, ref velocity, 0, rotateSpring);
        rb.angularVelocity = velocity;

        if (state == AIState.STUNNED)
            return;

        int tempLayer = gameObject.layer;
        gameObject.layer = 2;
        Collider2D[] nearWalls = Physics2D.OverlapCircleAll(transform.position, avoidanceDist, avoidanceMask);
        gameObject.layer = tempLayer;

        if (nearWalls.Length <= 0)
            return;

        Vector2 nearestPoint = Vector2.zero;
        float nearestDist = Mathf.Infinity;
        foreach (Collider2D wall in nearWalls)
        {
            Vector2 newPoint = wall.ClosestPoint(transform.position);
            float newDist = ((Vector2)transform.position - newPoint).sqrMagnitude;
            if (newDist < nearestDist && newDist > 0.125f)
            {
                nearestPoint = newPoint;
                nearestDist = newDist;
                rb.linearVelocity = CMath.TryAdd2(rb.linearVelocity, avoidanceAccel * speedStat.value * Time.deltaTime *
                    ((Vector2)transform.position - nearestPoint).normalized, avoidanceSpeed * speedStat.value);
            }
        }

        rb.linearVelocity = CMath.TryAdd2(rb.linearVelocity, avoidanceAccel * speedStat.value * Time.deltaTime *
            ((Vector2)transform.position - nearestPoint).normalized, avoidanceSpeed * speedStat.value);
    }

    public override void IdleUpdate()
    {
        base.IdleUpdate();

        desiredDir = Vector2.down;

        idleMoveTimer = Mathf.Max(0, idleMoveTimer - Time.deltaTime);
        if (idleMoveTimer <= 0)
        {
            idleMoving ^= true;
            RandomizeIdleMove();
        }

        if (idleMoving)
            rb.linearVelocity = CMath.TryAdd2(rb.linearVelocity, Time.deltaTime * idleMoveAccel * speedStat.value
                * idleMoveDir, idleMoveSpeed * speedStat.value);
        else
            rb.linearVelocity = CMath.TrySub2(rb.linearVelocity, Time.deltaTime * idleMoveAccel * speedStat.value);
    }

    public override void ActiveUpdate()
    {
        if (shooting)
        {
            rb.linearVelocity = CMath.TrySub2(rb.linearVelocity, Time.deltaTime * idleMoveAccel * speedStat.value);
            return;
        }
        base.ActiveUpdate();
        if (!playerVisible || state != AIState.ACTIVE)
        {
            desiredDir = Vector2.down;
            projectileWaitTimer = projectileWaitTime;
            return;
        }

        Vector2 playerDir = (trackedPlayer.position - transform.position).normalized;

        SpringUtils.CalcDampedSpringMotionParams(ref chaseSpring, Time.deltaTime, chaseSpringFrequency, chaseSpringDamping);

        Vector2 pos = transform.position, vel = rb.linearVelocity,
            desiredPos = (Vector2)trackedPlayer.position - playerDir * chaseDistance;

        SpringUtils.UpdateDampedSpringMotion(ref pos.x, ref vel.x, desiredPos.x, chaseSpring);
        SpringUtils.UpdateDampedSpringMotion(ref pos.x, ref vel.x, desiredPos.x, chaseSpring);

        rb.linearVelocity = vel;

        projectileWaitTimer = Mathf.Max(0, projectileWaitTimer - Time.deltaTime);
        if (projectileWaitTimer > 0)
        {
            desiredDir = Vector2.down;
            return;
        }

        desiredDir = playerDir;

        shooting = true;
        projectileIndicator.DOScale(projectile.radius, projectileFireTime).OnComplete(() =>
        {
            projectileIndicator.localScale = Vector3.zero;
            shooting = false;
            stunTimer = projectileStunTime;
            SetState(AIState.STUNNED);

            ProjectileInst projectileInst =
                Instantiate(projectilePrefab, projectileIndicator.position, transform.rotation).
                GetComponent<ProjectileInst>();
            projectileInst.Init(projectile, -transform.up);
            projectileInst.data.hit.damage = damageStat.value;

            rb.linearVelocity += (Vector2)transform.up * projectileRecoil;
        });
    }

    public override void ActiveEnter()
    {
        base.ActiveEnter();
        shooting = false;
        projectileWaitTimer = projectileWaitTime;
    }
}
