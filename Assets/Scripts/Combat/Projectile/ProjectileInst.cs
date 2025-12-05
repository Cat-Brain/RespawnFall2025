using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class ProjectileInst : MonoBehaviour
{
    public Collider2D col;
    public Rigidbody2D rb;
    public Projectile data;
    public List<SpriteRenderer> spriteRends;
    public List<LineRenderer> lineRends;

    [HideInInspector] public Vector2 direction;
    [HideInInspector] public float remainingRange;
    [HideInInspector] public Vector2 lastPos;

    public void Init(Projectile data, Vector2 direction)
    {
        foreach (SpriteRenderer sr in spriteRends)
            sr.color = data.color;
        foreach (LineRenderer lr in lineRends)
            lr.endColor = data.color;

        transform.localScale = data.radius * 0.01f * Vector3.one;
        transform.DOScale(data.radius, data.spawnTime);

        lastPos = transform.position;
        this.direction = direction;
        this.data = Instantiate(data);
        this.data.Init(this);
        OnInit();
    }

    public void Init(Vector2 direction)
    {
        Init(data, direction);
    }

    public virtual void OnInit()
    {
        SetDir(direction);
    }

    public void FixedUpdate()
    {
        data.OnUpdate();
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

    public void SetDir(Vector2 dir)
    {
        direction = dir;
        rb.linearVelocity = direction * data.speed;
    }
}
