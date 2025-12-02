using DG.Tweening;
using UnityEngine;

public class Gnat : Enemy
{
    [HideInInspector] public Rigidbody2D rb;

    public Projectile projectile;
    public GameObject projectilePrefab;
    public Transform leftWing, rightWing, projectileIndicator;

    public float projectileFireTime, projectileStunTime, projectileWaitTime, projectileRecoil;
    public float minWingRot, maxWingRot, wingRotFrequency;
    public float normalGravity, stunnedGravity;

    public float wallAvoidanceDist, wallAvoidanceAccel, wallAvoidanceSpeed;

    public float minIdleMoveTime, maxIdleMoveTime,
        minIdleMoveSpeed, maxIdleMoveSpeed, idleMoveAccel;

    public float chaseSpringFrequency, chaseSpringDamping, chaseDistance;

    public Vector2 desiredDir;

    public bool shooting;
    public float projectileWaitTimer, projectileFireTimer;
    public float idleMoveTimer, idleMoveSpeed;
    public Vector2 idleMoveDir;
    public bool idleMoving; // True means Moving, false means still

    public float wingRotTimer;
    public SpringUtils.tDampedSpringMotionParams chaseSpring = new();

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
        Collider2D[] nearWalls = Physics2D.OverlapCircleAll(transform.position, wallAvoidanceDist, wallMask);

        if (nearWalls.Length <= 0)
            return;

        Vector2 nearestPoint = nearWalls[0].ClosestPoint(transform.position);
        float nearestDist = ((Vector2)transform.position - nearestPoint).sqrMagnitude;
        foreach (Collider2D wall in nearWalls[1..])
        {
            Vector2 newPoint = wall.ClosestPoint(transform.position);
            float newDist = ((Vector2)transform.position - nearestPoint).sqrMagnitude;
            if (newDist < nearestDist)
            {
                nearestPoint = newPoint;
                nearestDist = newDist;
            }
        }

        rb.linearVelocity = CMath.TryAdd2(rb.linearVelocity, wallAvoidanceAccel * Time.deltaTime *
            ((Vector2)transform.position - nearestPoint).normalized, wallAvoidanceSpeed);
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
            rb.linearVelocity = CMath.TryAdd2(rb.linearVelocity, Time.deltaTime * idleMoveAccel
                * idleMoveDir, idleMoveSpeed);
        else
            rb.linearVelocity = CMath.TrySub2(rb.linearVelocity, Time.deltaTime * idleMoveAccel);
    }

    public override void ActiveUpdate()
    {
        if (shooting)
        {
            rb.linearVelocity = CMath.TrySub2(rb.linearVelocity, Time.deltaTime * idleMoveAccel);
            return;
        }
        base.ActiveUpdate();
        if (!playerVisible || state != AIState.ACTIVE)
        {
            desiredDir = Vector2.down;
            projectileFireTimer = projectileFireTime;
            return;
        }

        desiredDir = (trackedPlayer.position - transform.position).normalized;

        SpringUtils.CalcDampedSpringMotionParams(ref chaseSpring, Time.deltaTime, chaseSpringFrequency, chaseSpringDamping);

        Vector2 pos = transform.position, vel = rb.linearVelocity,
            desiredPos = (Vector2)trackedPlayer.position - desiredDir * chaseDistance;

        SpringUtils.UpdateDampedSpringMotion(ref pos.x, ref vel.x, desiredPos.x, chaseSpring);
        SpringUtils.UpdateDampedSpringMotion(ref pos.x, ref vel.x, desiredPos.x, chaseSpring);

        rb.linearVelocity = vel;

        projectileFireTimer = Mathf.Max(0, projectileFireTimer - Time.deltaTime);
        if (projectileFireTimer > 0)
            return;

        shooting = true;
        projectileIndicator.DOScale(projectile.radius, projectileFireTime).OnComplete(() =>
        {
            projectileIndicator.localScale = Vector3.zero;
            shooting = false;
            stunTimer = projectileStunTime;
            SetState(AIState.STUNNED);
            Instantiate(projectilePrefab, projectileIndicator.position, transform.rotation).
            GetComponent<ProjectileInst>().Init(projectile, desiredDir);
            rb.linearVelocity -= desiredDir * projectileRecoil;
        });
    }

    public override void ActiveEnter()
    {
        base.ActiveEnter();
        shooting = false;
        projectileFireTimer = projectileFireTime;
    }
}
