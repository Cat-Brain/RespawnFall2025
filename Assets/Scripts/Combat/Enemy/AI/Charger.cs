using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CapsuleRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Charger : Enemy
{
    [HideInInspector] public CapsuleRenderer cr;
    [HideInInspector] public Rigidbody2D rb;

    public Hit hit;

    public Color idleColor, activeColor, chargeColor, stunnedColor;

    public float idleColorTweenTime, activeColorTweenTime;

    public float preChargeTime, chargeScaleTime, chargeStunTime;

    public Vector2 defaultDim, preChargeDim, chargingDim;

    public float minIdleWalkTime, maxIdleWalkTime,
        minIdleWalkSpeed, maxIdleWalkSpeed, idleWalkAccel;

    public float activeWalkSpeed, activeWalkAccel;
    public float chargeSpeed, chargeAccel;

    public float maxChargeOffset;

    public float idleToActiveHopForce;
    public Vector2 chargeHitHopForce;

    public float idleBumpCheckRadius, chargeHitCheckRadius;
    public Vector2 idleBumpCheckOffset, chargeHitCheckOffset;
    public LayerMask idleBumpCheckMask, chargeHitMask;

    public float idleWalkTimer, idleWalkSpeed;
    public bool idleWalking; // True means walking, false means still
    public float stunTimer;

    public bool walkingRight;
    public bool charging;

    public Transform trackedPlayer;

    public enum AIState
    {
        IDLE, ACTIVE, CHARGING, STUNNED
    }

    public AIState state;

    public void Awake()
    {
        cr = GetComponent<CapsuleRenderer>();
        rb = GetComponent<Rigidbody2D>();

        state = AIState.IDLE;
        idleWalking = false;
        RandomizeIdleWalk();
        cr.color = idleColor;

        transform.localScale = CMath.Vector3XY_Z(defaultDim, 1);
    }

    void Update()
    {
        switch (state)
        {
            case AIState.IDLE:
                idleWalkTimer = Mathf.Max(0, idleWalkTimer - Time.deltaTime);
                if (idleWalkTimer <= 0)
                {
                    idleWalking ^= true;
                    RandomizeIdleWalk();
                }
                int tempLayer = gameObject.layer;
                gameObject.layer = 2;
                if (Physics2D.OverlapCircle(IdleBumpCheckPos(), idleBumpCheckRadius, idleBumpCheckMask))
                    walkingRight ^= true;
                gameObject.layer = tempLayer;
                if (idleWalking)
                    rb.linearVelocityX = CMath.TryAdd(rb.linearVelocityX,
                        Time.deltaTime * (walkingRight ? idleWalkAccel : -idleWalkAccel), idleWalkSpeed);
                else
                    rb.linearVelocityX = CMath.TrySub(rb.linearVelocityX, Time.deltaTime * idleWalkAccel);

                Collider2D foundPlayer;
                if (foundPlayer = FindPlayer())
                {
                    trackedPlayer = foundPlayer.transform;
                    SetState(AIState.ACTIVE);
                }
                break;
            case AIState.ACTIVE:
                if (trackedPlayer == null)
                {
                    SetState(AIState.IDLE);
                    break;
                }
                if (Mathf.Abs(transform.position.y - trackedPlayer.position.y) <= maxChargeOffset
                    && PositionVisible(trackedPlayer.position))
                {
                    SetState(AIState.CHARGING);
                    break;
                }
                walkingRight = trackedPlayer.position.x > transform.position.x;
                rb.linearVelocityX = CMath.TryAdd(rb.linearVelocityX,
                    Time.deltaTime * (walkingRight ? activeWalkAccel : -activeWalkAccel), activeWalkSpeed);

                break;
            case AIState.CHARGING:
                if (charging)
                {
                    Collider2D[] overlaps = Physics2D.OverlapCircleAll(ChargeHitCheckPos(), chargeHitCheckRadius, chargeHitMask);
                    foreach (Collider2D overlap in overlaps)
                    {
                        if (overlap.transform == transform)
                            continue;
                        if (overlap.TryGetComponent(out HealthInst health))
                            health.ApplyHit(hit);
                        SetState(AIState.STUNNED);
                    }
                    if (state != AIState.CHARGING)
                        break;

                    rb.linearVelocityX = CMath.TryAdd(rb.linearVelocityX,
                        Time.deltaTime * (walkingRight ? chargeAccel : -chargeAccel), chargeSpeed);
                }
                else
                    rb.linearVelocityX = CMath.TrySub(rb.linearVelocityX, Time.deltaTime * chargeAccel);
                break;
            case AIState.STUNNED:
                stunTimer = Mathf.Max(0, stunTimer - Time.deltaTime);
                if (stunTimer <= 0)
                {
                    if (trackedPlayer && PositionVisible(trackedPlayer.position))
                    {
                        SetState(AIState.ACTIVE);
                        break;
                    }
                    Collider2D newPlayer;
                    if (newPlayer = FindPlayer())
                    {
                        trackedPlayer = newPlayer.transform;
                        SetState(AIState.ACTIVE);
                        break;
                    }
                    SetState(AIState.IDLE);
                }
                break;
        }
    }

    public void RandomizeIdleWalk()
    {
        idleWalkTimer = Random.Range(minIdleWalkTime, maxIdleWalkTime);
        if (!idleWalking)
            return;
        idleWalkSpeed = Random.Range(minIdleWalkSpeed, maxIdleWalkSpeed);
        walkingRight = Random.Range(0, 2) == 0;
    }

    [ProPlayButton]
    public void SetState(AIState state)
    {
        if (this.state == state) return;

        switch (state)
        {
            case AIState.IDLE:
                cr.DOColor(idleColor, idleColorTweenTime);
                idleWalking = false;
                RandomizeIdleWalk();
                break;
            case AIState.ACTIVE:
                cr.DOColor(activeColor, activeColorTweenTime);
                rb.linearVelocityY = Mathf.Max(0, rb.linearVelocityY) + idleToActiveHopForce;
                break;
            case AIState.CHARGING:
                cr.DOColor(chargeColor, preChargeTime);
                charging = false;
                transform.DOScale(CMath.Vector3XY_Z(preChargeDim, 1), preChargeTime).OnComplete(() =>
                {
                    charging = true;
                    transform.DOScale(CMath.Vector3XY_Z(chargingDim, 1), chargeScaleTime);
                });
                break;
            case AIState.STUNNED:
                cr.DOColor(stunnedColor, chargeStunTime);
                transform.DOScale(CMath.Vector3XY_Z(defaultDim, 1), chargeStunTime);
                rb.linearVelocity = chargeHitHopForce;
                if (!walkingRight)
                    rb.linearVelocityX *= -1;
                stunTimer = chargeStunTime;
                break;
        }
        this.state = state;
    }

    public Vector2 IdleBumpCheckPos()
    {
        return transform.position + transform.up * idleBumpCheckOffset.y + transform.right *
            (walkingRight ? idleBumpCheckOffset.x : -idleBumpCheckOffset.x);
    }

    public Vector2 ChargeHitCheckPos()
    {
        return transform.position + transform.up * chargeHitCheckOffset.y + transform.right *
            (walkingRight ? chargeHitCheckOffset.x : -chargeHitCheckOffset.x);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = state == AIState.IDLE ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(IdleBumpCheckPos(), idleBumpCheckRadius);
        Gizmos.color = state == AIState.CHARGING ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(ChargeHitCheckPos(), chargeHitCheckRadius);
    }
}
