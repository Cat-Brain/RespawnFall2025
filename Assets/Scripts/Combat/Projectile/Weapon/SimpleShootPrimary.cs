using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New SimpleShootPrimary", menuName = "Weapons/Primaries/SimpleShootPrimary")]
public class SimpleShootPrimary : StandardWeaponPrimary
{
    public GameObject projectilePrefab;
    public Projectile projectile;

    public float fireRate;
    protected float timeTillShoot = 0;

    public virtual Projectile GetProjectile()
    {
        Projectile result = Instantiate(projectile);
        result.hit = weapon.GetHit();
        return result;
    }

    public override void Upd(bool activated)
    {
        if (timeTillShoot > 0)
            timeTillShoot -= Time.deltaTime;

        if (activated && timeTillShoot <= 0 && weapon.reload.remainingAmmo > 0)
            OnActivate();

        timeTillShoot = Mathf.Max(0, timeTillShoot);
    }

    public override void OnActivate()
    {
        timeTillShoot += 1 / (fireRate * weapon.stats.fireRate);
        Instantiate(projectilePrefab, weapon.wielder.position, Quaternion.identity)
            .GetComponent<ProjectileInst>().Init(GetProjectile(),
            ((Vector2)(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - weapon.wielder.position)).normalized);
        base.OnActivate();
    }
}
