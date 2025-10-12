using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyHitbox))]
public class SwooperEnemy : MonoBehaviour
{
    public FlipToDirection flip;

    public Collider2D swoopTrigger;

    public float riseTime, swoopTime, descendTime;
    public float riseHeight, descendHeight;

    public Vector2 basePos;
    public float elapsedSwoop;
    public bool swoopLeft;

    private Rigidbody2D rb;
    private EnemyHitbox hitbox;
    public AudioClip harpyClip;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<EnemyHitbox>();

        basePos = transform.position;

        if (basePos.x > 0)
        {
            basePos.x *= -1;
            SetDir(true);
        }
        else
            SetDir(false);

        PrepareForSwoop();
    }

    void FixedUpdate()
    {
        if (elapsedSwoop != -1)
        {
            elapsedSwoop += Time.fixedDeltaTime;
            if (elapsedSwoop > TotalTime())
            {
                SetDir(!swoopLeft);
                PrepareForSwoop();
            }
        }
        transform.position = DesiredPos();
        rb.linearVelocity = DesiredVel();
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (elapsedSwoop == -1 && collider.CompareTag(hitbox.playerTag))
            ActivateSwoop();
    }

    public void SetDir(bool left)
    {
        if (left)
        {
            swoopLeft = true;
            flip.direction = EntityDirection.LEFT;
            return;
        }
        swoopLeft = false;
        flip.direction = EntityDirection.RIGHT;
    }

    public void ActivateSwoop()
    {
        elapsedSwoop = 0;

        swoopTrigger.enabled = false;
        hitbox.enabled = true;
        AudioManager.instance.PlaySoundFXClip(harpyClip, transform, 0.1f);
    }

    public void PrepareForSwoop()
    {
        elapsedSwoop = -1;

        swoopTrigger.enabled = true;
        hitbox.enabled = false;
    }

    public Vector2 DesiredPos()
    {
        Vector2 result = basePos + SwoopOffset();
        if (swoopLeft)
            result.x *= -1;
        return result;
    }

    public Vector2 SwoopOffset()
    {
        if (elapsedSwoop == -1)
            return Vector2.zero;

        float t = elapsedSwoop;
        if (t < riseTime)
            return t / riseTime * riseHeight * Vector2.up;
        t -= riseTime;
        if (t < swoopTime)
            return new Vector2(t / swoopTime * basePos.x * -2, riseHeight +
                4 * descendHeight * t / swoopTime * (t / swoopTime - 1));
        t -= swoopTime;
        return new Vector2(basePos.x * -2, (1 - t / riseTime) * riseHeight);
    }

    public Vector2 DesiredVel()
    {
        if (elapsedSwoop == -1)
            return Vector2.zero;

        float t = elapsedSwoop;
        if (t < riseTime)
            return riseHeight / riseTime * Vector2.up;
        t -= riseTime;
        if (t < swoopTime)
            return new Vector2(basePos.x * 2 / swoopTime * (swoopLeft ? 1 : -1), 4 * descendHeight / swoopTime * (2 * t / swoopTime - 1));
        t -= swoopTime;
        return riseHeight / riseTime * Vector2.down;
    }

    public float TotalTime()
    {
        return riseTime + swoopTime + descendTime;
    }
}
