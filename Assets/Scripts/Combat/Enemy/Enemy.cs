using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

[RequireComponent(typeof(TickEntity))]
public class Enemy : OnTickEffect
{
    public int tier;
    public EnemyType type;
    [Tooltip("Offset added to position at spawn to make bottom of enemy grid alligned")]
    public Vector2 spawnOffset;
    public Vector2 spawnDimensions;
    public LayerMask playerMask, wallMask;
    public float sightDist, startleTime, interestTime;

    [HideInInspector] public EnemyManager enemyManager;

    [HideInInspector] public float startleTimer, interestTimer, stunTimer;
    [HideInInspector] public Transform trackedPlayer;
    [HideInInspector] public Collider2D trackedPlayerCol;
    [HideInInspector] public bool playerVisible;

    public enum AIState
    {
        NO_STATE, IDLE, STARTLED, ACTIVE, STUNNED
    }

    /*[HideInInspector]*/ public AIState state;

    public Collider2D FindPlayerCollider()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, sightDist, playerMask))
            if (collider.enabled && PositionVisible(collider.transform.position))
                return collider;
        return null;
    }

    public bool FindPlayer()
    {
        if (!(trackedPlayerCol = FindPlayerCollider()))
            return false;

        trackedPlayer = trackedPlayerCol.transform;
        return true;
    }

    public bool PositionVisible(Vector3 position)
    {
        return !Physics2D.Linecast(transform.position, position, wallMask);
    }

    public bool PlayerVisible()
    {
        return trackedPlayer && PositionVisible(trackedPlayer.position);
    }

    public void ForgetPlayer()
    {
        trackedPlayer = null;
        trackedPlayerCol = null;
    }

    [ProPlayButton]
    public void SetState(AIState state)
    {
        if (this.state == state) return;

        switch (state)
        {
            case AIState.IDLE:
                IdleEnter();
                break;
            case AIState.STARTLED:
                StartledEnter();
                break;
            case AIState.ACTIVE:
                ActiveEnter();
                break;
            case AIState.STUNNED:
                StunnedEnter();
                break;
        }
        this.state = state;
    }

    public void Awake()
    {
        SetState(AIState.IDLE);
    }

    void Update()
    {
        playerVisible = PlayerVisible();

        switch (state)
        {
            case AIState.IDLE:
                IdleUpdate();
                break;
            case AIState.STARTLED:
                StartledUpdate();
                break;
            case AIState.ACTIVE:
                ActiveUpdate();
                break;
            case AIState.STUNNED:
                StunnedUpdate();
                break;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = PlayerVisible() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, sightDist);
    }

    public virtual void IdleUpdate()
    {
        if (FindPlayer())
            SetState(AIState.STARTLED);
    }

    public virtual void StartledUpdate()
    {
        startleTimer = Mathf.Max(0, startleTimer - Time.deltaTime);

        if (startleTimer <= 0)
            SetState(AIState.ACTIVE);
    }

    public virtual void ActiveUpdate()
    {
        if (playerVisible)
            interestTimer = interestTime;
        else
            interestTimer = Mathf.Max(0, interestTimer - Time.deltaTime);
        if (interestTimer <= 0)
        {
            ForgetPlayer();
            SetState(AIState.IDLE);
        }
    }

    public virtual void StunnedUpdate()
    {
        stunTimer = Mathf.Max(0, stunTimer - Time.deltaTime);
        if (stunTimer <= 0)
            SetState(playerVisible || FindPlayer() ? AIState.ACTIVE : AIState.IDLE);
    }

    public virtual void IdleEnter() { }
    public virtual void StartledEnter()
    {
        startleTimer = startleTime;
    }
    public virtual void ActiveEnter() { }
    public virtual void StunnedEnter() { }

    public override void OnTick(TickEntity tickEntity)
    {
        enemyManager.RemoveEnemy(this);
    }
}
