using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponReload", menuName = "Weapons/WeaponReload")]
public class WeaponReload : ScriptableObject
{
    public int ammo;
    public float reloadTime;
    public AudioClip reloadClip;

    [HideInInspector] public Weapon weapon;

    [HideInInspector] public bool reloading;
    [HideInInspector] public int remainingAmmo;
    [HideInInspector] public float reloadTimer;

    public virtual void Str(Weapon weapon)
    {
        this.weapon = weapon;
        reloading = false;
        remainingAmmo = ammo;
        reloadTimer = 0;
    }

    public virtual void Upd(bool activated)
    {
        if (reloading)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer > 0)
                return;
            remainingAmmo = Mathf.RoundToInt(ammo * weapon.stats.magazineSize);
            reloading = false;
            return;
        }
        if (!activated && remainingAmmo > 0)
            return;

        reloading = true;
        reloadTimer = reloadTime / weapon.stats.reloadSpeed;
        AudioManager.instance.PlaySoundFXClip(reloadClip, weapon.wielder, 1);
    }
}
