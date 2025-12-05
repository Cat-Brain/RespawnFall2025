using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New BurstWeaponPrimary", menuName = "Weapons/Primaries/BurstWeaponPrimary")]
public class BurstWeaponPrimary : StandardWeaponPrimary
{
    public GameObject projectilePrefab;
    public Projectile projectile;

    public int shotsPerBurst;
    protected int shotsFiredThisBurst = 0;
    public float fireRate, timeBetweenBursts;
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

        if ((activated || shotsFiredThisBurst != 0) && timeTillShoot <= 0 && weapon.reload.remainingAmmo > 0)
            OnActivate();

        timeTillShoot = Mathf.Max(0, timeTillShoot);
    }

    public override void OnActivate()
    {
        shotsFiredThisBurst++;
        if (shotsFiredThisBurst >= shotsPerBurst)
        {
            timeTillShoot += timeBetweenBursts / weapon.stats.fireRate;
            shotsFiredThisBurst = 0;
            weapon.reload.remainingAmmo--;
        }
        else
            timeTillShoot += 1 / (fireRate * weapon.stats.fireRate);

        Instantiate(projectilePrefab, weapon.wielder.position, Quaternion.identity)
            .GetComponent<ProjectileInst>().Init(GetProjectile(),
            ((Vector2)(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - weapon.wielder.position)).normalized);
    }
}
