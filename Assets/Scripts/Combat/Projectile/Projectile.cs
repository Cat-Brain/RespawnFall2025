using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectiles/Projectile")]
public class Projectile : ScriptableObject
{
    public Color color = Color.white;
    public Hit hit;
    public LayerMask targetMask, destroyedByMask;
    public float speed, range, spread;
    public float destructTime;

    [HideInInspector] public ProjectileInst inst;

    public virtual void Init(ProjectileInst inst)
    {
        this.inst = inst;
        inst.remainingRange = range;
        inst.direction = CMath.Rotate(inst.direction, Mathf.Deg2Rad * Random.Range(-spread, spread));
    }
}
