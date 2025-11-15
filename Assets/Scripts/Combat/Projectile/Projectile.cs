using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;

    public Hit hit;
    public LayerMask targetMask, destroyedByMask;
    public float speed;

    public float destructTime;

    protected Vector2 direction;


    public void Init(Vector2 direction)
    {
        this.direction = direction;
        OnInit();
    }

    public virtual void OnInit()
    {
        transform.rotation = CMath.LookDir(direction);
        rb.linearVelocity = direction * speed;
    }

    public virtual void Destruct()
    {
        transform.DOScale(0, destructTime).OnComplete(() => Destroy(gameObject));
        enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (enabled && collider.enabled &&
            CMath.LayerOverlapsMask(collider.gameObject.layer, destroyedByMask))
            Destruct();
    }
}
