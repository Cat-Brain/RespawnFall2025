using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;

    public Hit hit;
    public LayerMask targetMask;
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
        rb.linearVelocity = direction * speed;
    }

    public virtual void Destruct()
    {
        Destroy(gameObject, destructTime);
        transform.DOScale(0, destructTime);
        enabled = false;
    }
}
