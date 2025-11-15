using UnityEngine;
using DG.Tweening;

public class ProjectileInst : MonoBehaviour
{
    public Rigidbody2D rb;
    public Projectile data;

    [HideInInspector] public Vector2 direction;
    [HideInInspector] public float remainingRange;
    [HideInInspector] public Vector2 lastPos;

    public void Init(Projectile data, Vector2 direction)
    {
        lastPos = transform.position;
        this.direction = direction;
        this.data = data;
        data.Init(this);
        OnInit();
    }

    public void Init(Vector2 direction)
    {
        lastPos = transform.position;
        this.direction = direction;
        data = Instantiate(data);
        data.Init(this);
        OnInit();
    }

    public virtual void OnInit()
    {
        rb.linearVelocity = direction * data.speed;
        transform.rotation = CMath.LookDir(direction);
    }

    public void FixedUpdate()
    {
        remainingRange -= Vector2.Distance(lastPos, transform.position);
        lastPos = transform.position;
        if (remainingRange < 0)
            Destruct();
    }

    public virtual void Destruct()
    {
        transform.DOScale(0, data.destructTime).OnComplete(() => Destroy(gameObject));
        enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (enabled && collider.enabled &&
            CMath.LayerOverlapsMask(collider.gameObject.layer, data.destroyedByMask))
            Destruct();
    }
}
