using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum HitResult
{
    HIT, ABSORBED, BLOCKED
}

[RequireComponent(typeof(PlayerWeaponInstance), typeof(PlayerMove))]
[RequireComponent(typeof(PlayerGravity), typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerManager : MonoBehaviour
{
    public FlipToDirection playerFlip;
    public List<Renderer> rends;

    public InputActionReference moveAction, jumpAction;

    public float groundedRadius, groundedDistance;
    public LayerMask groundedMask;

    public Vector2 recoilMultiplier;

    public EntityDirection direction;

    public string winZoneTag;

    [HideInInspector] public bool active = true;
    [HideInInspector] public float stunInvulnerability = 0, moveStun = 0; // Applies to both horizontal movement and jumping
    [HideInInspector] public string sortingLayer;
    
    [HideInInspector] public GameManager gameManager;

    [HideInInspector] public PlayerWeaponInstance playerWeapon;
    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public PlayerGravity playerGravity;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Collider2D col;

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.playerManager = this;

        playerWeapon = GetComponent<PlayerWeaponInstance>();
        playerMove = GetComponent<PlayerMove>();
        playerGravity = GetComponent<PlayerGravity>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        sortingLayer = rends[0].sortingLayerName;
    }

    void Update()
    {
        if (!active)
        {
            playerWeapon.enabled = false;
            playerMove.enabled = false;
            playerGravity.enabled = false;
            col.enabled = false;
        }
        else if (gameManager.gameState != GameState.IN_GAME)
        {
            playerWeapon.enabled = false;
            playerMove.enabled = false;
        }
        else
        {
            playerWeapon.enabled = true;
            playerMove.enabled = true;
            playerGravity.enabled = true;
            col.enabled = true;
        }
        stunInvulnerability = Mathf.Max(0, stunInvulnerability - Time.deltaTime);
        if (moveStun != -1)
            moveStun = Mathf.Max(0, moveStun - Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag(winZoneTag))
            gameManager.PlayerWin();
    }

    public void SetSortingLayer(string sortingLayer = "")
    {
        sortingLayer = sortingLayer == "" ? this.sortingLayer : sortingLayer;
        foreach (Renderer rend in rends)
            rend.sortingLayerName = sortingLayer;
    }

    public bool IsGrounded()
    {
        int tempLayer = gameObject.layer;
        gameObject.layer = 2; // Ignore Raycasts

        bool successful = Physics2D.CircleCast(transform.position, groundedRadius, Vector2.down, groundedDistance, groundedMask);

        gameObject.layer = tempLayer;

        return successful;
    }

    public HitResult TryHit(Vector2 knockbackForce, float duration, float invulnerability)
    {
        rb.linearVelocity += knockbackForce;

        if (stunInvulnerability > 0)
            return HitResult.ABSORBED;

        stunInvulnerability = invulnerability;
        moveStun += duration;
        return HitResult.HIT;
    }

    // If the player does not have i frames then stun them and returns true. Else returns false
    public bool TryAddStun(float duration, float invulnerability)
    {
        if (stunInvulnerability > 0)
            return false;

        stunInvulnerability = invulnerability;
        moveStun += duration;
        return true;
    }

    // The direction should be pointing where the player is being pushed. This will be the inverse of the direction aimed at
    public void ApplyRecoil(Vector2 direction, float force)
    {
        direction = direction.normalized;
        rb.linearVelocityX += direction.x * force * recoilMultiplier.y;
        if (direction.y > 0)
        {
            float recoilVelocity = direction.y * force * recoilMultiplier.y;
            if (recoilVelocity < rb.linearVelocityY)
                return;
            rb.linearVelocityY = recoilVelocity;
            playerMove.tapJumpTimer = 0;
        }
    }

    public void SetDirection(EntityDirection direction)
    {
        this.direction = direction;
        playerFlip.direction = direction;
    }

    public bool Stunned()
    {
        return moveStun > 0 || moveStun == -1;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.down * groundedDistance, groundedRadius);
    }
}
