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

    public float startledDecel, activeWalkSpeed, activeWalkAccel;
    public float chargeSpeed, chargeAccel;

    public float maxChargeOffset;

    public float idleToActiveHopForce;
    public Vector2 chargeHitHopForce;

    public float idleBumpCheckRadius, chargeHitCheckRadius;
    public Vector2 idleBumpCheckOffset, chargeHitCheckOffset;
    public LayerMask idleBumpCheckMask, chargeHitMask;

    public float idleWalkTimer, idleWalkSpeed;
    public bool idleWalking; // True means walking, false means still

    public bool walkingRight;
    public bool charging;

    public enum ChargerAIState
    {
        INACTIVE, ACTIVE, CHARGING
    }

    public ChargerAIState chargerState;

    public void RandomizeIdleWalk()
    {
        idleWalkTimer = Random.Range(minIdleWalkTime, maxIdleWalkTime);
        if (!idleWalking)
            return;
        idleWalkSpeed = Random.Range(minIdleWalkSpeed, maxIdleWalkSpeed);
        walkingRight = Random.Range(0, 2) == 0;
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

    public bool WantsCharge()
    {
        return playerVisible && Mathf.Abs(transform.position.y - trackedPlayer.position.y) <= maxChargeOffset;
    }

    [ProPlayButton]
    public void SetChargerState(ChargerAIState state)
    {
        if (chargerState == state) return;

        switch (state)
        {
            case ChargerAIState.ACTIVE:
                cr.DOColor(activeColor, activeColorTweenTime);
                break;
            case ChargerAIState.CHARGING:
                cr.DOColor(chargeColor, preChargeTime);
                charging = false;
                transform.DOScale(CMath.Vector3XY_Z(preChargeDim, 1), preChargeTime).OnComplete(() =>
                {
                    charging = true;
                    transform.DOScale(CMath.Vector3XY_Z(chargingDim, 1), chargeScaleTime);
                });
                break;
        }
        chargerState = state;
    }

    public new void Awake()
    {
        cr = GetComponent<CapsuleRenderer>();
        rb = GetComponent<Rigidbody2D>();

        base.Awake();
        idleWalking = false;
        RandomizeIdleWalk();
        cr.color = idleColor;

        transform.localScale = CMath.Vector3XY_Z(defaultDim, 1);
    }

    public new void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = state == AIState.IDLE ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(IdleBumpCheckPos(), idleBumpCheckRadius);
        Gizmos.color = chargerState == ChargerAIState.CHARGING ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(ChargeHitCheckPos(), chargeHitCheckRadius);
    }

    public override void IdleUpdate()
    {
        int tempLayer = gameObject.layer;
        gameObject.layer = 2;
        if (Physics2D.OverlapCircle(IdleBumpCheckPos(), idleBumpCheckRadius, idleBumpCheckMask))
            walkingRight ^= true;
        gameObject.layer = tempLayer;

        idleWalkTimer = Mathf.Max(0, idleWalkTimer - Time.deltaTime);
        if (idleWalkTimer <= 0)
        {
            idleWalking ^= true;
            RandomizeIdleWalk();
        }

        if (idleWalking)
            rb.linearVelocityX = CMath.TryAdd(rb.linearVelocityX,
                Time.deltaTime * (walkingRight ? idleWalkAccel : -idleWalkAccel), idleWalkSpeed);
        else
            rb.linearVelocityX = CMath.TrySub(rb.linearVelocityX, Time.deltaTime * idleWalkAccel);

        base.IdleUpdate();
    }

    public override void StartledUpdate()
    {
        rb.linearVelocityX = CMath.TrySub(rb.linearVelocityX, Time.deltaTime * startledDecel);
        base.StartledUpdate();
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        switch (chargerState)
        {
            case ChargerAIState.ACTIVE:
                if (WantsCharge())
                    SetChargerState(ChargerAIState.CHARGING);
                walkingRight = trackedPlayer.position.x > transform.position.x;
                rb.linearVelocityX = CMath.TryAdd(rb.linearVelocityX,
                    Time.deltaTime * (walkingRight ? activeWalkAccel : -activeWalkAccel), activeWalkSpeed);
                break;
            case ChargerAIState.CHARGING:
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
                    if (chargerState != ChargerAIState.CHARGING)
                        break;

                    rb.linearVelocityX = CMath.TryAdd(rb.linearVelocityX,
                        Time.deltaTime * (walkingRight ? chargeAccel : -chargeAccel), chargeSpeed);
                }
                else
                    rb.linearVelocityX = CMath.TrySub(rb.linearVelocityX, Time.deltaTime * chargeAccel);
                break;
        }
    }

    public override void IdleEnter()
    {
        SetChargerState(ChargerAIState.INACTIVE);
        cr.DOColor(idleColor, idleColorTweenTime);
        idleWalking = false;
        RandomizeIdleWalk();
        base.IdleEnter();
    }

    public override void StartledEnter()
    {
        SetChargerState(ChargerAIState.INACTIVE);
        rb.linearVelocityY = Mathf.Max(0, rb.linearVelocityY) + idleToActiveHopForce;
        base.StartledEnter();
    }

    public override void ActiveEnter()
    {
        SetChargerState(WantsCharge() ? ChargerAIState.CHARGING : ChargerAIState.ACTIVE);
        base.ActiveEnter();
    }

    public override void StunnedEnter()
    {
        SetChargerState(ChargerAIState.INACTIVE);
        cr.DOColor(stunnedColor, chargeStunTime);
        transform.DOScale(CMath.Vector3XY_Z(defaultDim, 1), chargeStunTime);
        rb.linearVelocity = chargeHitHopForce;
        if (!walkingRight)
            rb.linearVelocityX *= -1;
        stunTimer = chargeStunTime;
        base.StunnedEnter();
    }
}
