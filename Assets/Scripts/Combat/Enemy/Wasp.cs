using DG.Tweening;
using UnityEngine;

public class Wasp : Enemy
{
    [HideInInspector] public Rigidbody2D rb;

    public Projectile projectile;
    public GameObject projectilePrefab;
    public Transform leftWing, rightWing, bodyTransform, projectileIndicator;
    public EntityStat speedStat, damageStat;

    public int projectilesInBurst;
    public float minProjectileScale;
    public float projectileFireTime, projectileStunTime, projectileWaitTime, projectileRecoil;
    public float minWingRot, maxWingRot, wingRotFrequency;
    public float maxBodyRotation, bodyRotationMultiplier;
    public float normalGravity, stunnedGravity;

    public LayerMask avoidanceMask;
    public float avoidanceRadius, avoidanceDist, avoidanceAccel, avoidanceSpeed;

    public float minIdleMoveTime, maxIdleMoveTime,
        minIdleMoveSpeed, maxIdleMoveSpeed, idleMoveAccel;

    public float chaseSpringFrequency, chaseSpringDamping, chaseDistance;

    public float rotateSpringFrequency, rotateSpringDamping;

    public PhysicsMaterial2D normalPhyMat, stunnedPhyMat;

    public Vector2 desiredDir;

    public bool shooting;
    public int remainingBurst;
    public float projectileWaitTimer;
    public float idleMoveTimer, idleMoveSpeed;
    public Vector2 idleMoveDir;
    public bool idleMoving; // True means Moving, false means still
    public float bodyVelocity;

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
        leftWing.transform.rotation = Quaternion.Euler(0, 0, wingRot);
        rightWing.transform.rotation = Quaternion.Euler(0, 0, -wingRot);
    }

    public void FixedUpdate()
    {
        rb.sharedMaterial = state == AIState.STUNNED ? stunnedPhyMat : normalPhyMat;

        float mainRotation = Vector2.SignedAngle(Vector2.down, -transform.up), mainVelocity = rb.angularVelocity,
            desiredBodyRotation = DesiredBodyRotation(),
            bodyRotation = CMath.Rot0To360IntoN180To180(bodyTransform.eulerAngles.z - desiredBodyRotation);

        SpringUtils.CalcDampedSpringMotionParams(ref rotateSpring, Time.deltaTime, rotateSpringFrequency, rotateSpringDamping);
        SpringUtils.UpdateDampedSpringMotion(ref mainRotation, ref mainVelocity, 0, rotateSpring);
        SpringUtils.UpdateDampedSpringMotion(ref bodyRotation, ref bodyVelocity, 0, rotateSpring);
        rb.angularVelocity = mainVelocity;
        bodyTransform.rotation = Quaternion.Euler(0, 0, desiredBodyRotation + bodyRotation);
        projectileIndicator.rotation = CMath.LookDir(desiredDir, Vector2.right);


        if (state == AIState.STUNNED)
            return;

        int tempLayer = gameObject.layer;
        gameObject.layer = 2;
        RaycastHit2D nearWall = Physics2D.CircleCast(transform.position, avoidanceRadius, Vector2.down, avoidanceDist, avoidanceMask);
        gameObject.layer = tempLayer;

        if (!nearWall)
            return;

        rb.linearVelocity = CMath.TryAdd2(rb.linearVelocity, avoidanceAccel * speedStat.value * Time.deltaTime *
            ((Vector2)transform.position - nearWall.point).normalized, avoidanceSpeed * speedStat.value);
    }

    public float DesiredBodyRotation()
    {
        return Mathf.Clamp(Vector2.SignedAngle(Vector2.down, desiredDir) * bodyRotationMultiplier, -maxBodyRotation, maxBodyRotation);
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
        /*if (shooting)
        {
            rb.linearVelocity = CMath.TrySub2(rb.linearVelocity, Time.deltaTime * idleMoveAccel * speedStat.value);
            return;
        }*/
        if (!shooting)
            base.ActiveUpdate();
        if (!playerVisible || state != AIState.ACTIVE)
        {
            desiredDir = Vector2.down;
            projectileWaitTimer = projectileWaitTime;
            return;
        }

        Vector2 playerDir = (trackedPlayer.position - projectileIndicator.position).normalized;

        SpringUtils.CalcDampedSpringMotionParams(ref chaseSpring, Time.deltaTime, chaseSpringFrequency, chaseSpringDamping);

        Vector2 pos = transform.position, vel = rb.linearVelocity,
            desiredPos = (Vector2)trackedPlayer.position - playerDir * chaseDistance;

        SpringUtils.UpdateDampedSpringMotion(ref pos.x, ref vel.x, desiredPos.x, chaseSpring);
        SpringUtils.UpdateDampedSpringMotion(ref pos.x, ref vel.x, desiredPos.x, chaseSpring);

        rb.linearVelocity = vel;
        desiredDir = playerDir;

        projectileWaitTimer = Mathf.Max(0, projectileWaitTimer - Time.deltaTime);
        if (projectileWaitTimer > 0 || shooting)
            return;

        shooting = true;
        SpawnProjectile();
    }

    public override void ActiveEnter()
    {
        base.ActiveEnter();
        shooting = false;
        remainingBurst = projectilesInBurst;
        projectileWaitTimer = projectileWaitTime;
    }

    public void SpawnProjectile()
    {
        projectileIndicator.DOScale(projectile.radius, projectileFireTime).OnComplete(() =>
        {
            projectileIndicator.localScale = Vector3.one * minProjectileScale;

            ProjectileInst projectileInst =
                Instantiate(projectilePrefab, projectileIndicator.position, transform.rotation).
                GetComponent<ProjectileInst>();
            projectileInst.Init(projectile, desiredDir);
            projectileInst.data.hit.damage = damageStat.value;

            rb.linearVelocity -= (Vector2)desiredDir * projectileRecoil;
            remainingBurst--;
            if (remainingBurst <= 0)
            {
                shooting = false;
                stunTimer = projectileStunTime;
                SetState(AIState.STUNNED);
            }
            else
                SpawnProjectile();
        });
    }
}
