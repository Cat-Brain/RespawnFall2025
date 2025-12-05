using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public WeaponReload reload;
    public WeaponPrimary primary;
    public Hit baseHit;
    public WeaponStats stats;

    [HideInInspector] public Weapon baseWeapon;
    [HideInInspector] public Transform wielder;

    public virtual Weapon Copy(Transform wielder)
    {
        Weapon result = Instantiate(this);
        result.baseWeapon = this;
        result.stats = WeaponStats.DEFAULT;
        result.wielder = wielder;
        result.reload = Instantiate(reload);
        result.primary = Instantiate(primary);
        result.reload.Str(result);
        result.primary.Str(result);
        return result;
    }

    public virtual Hit GetHit()
    {
        Hit result = baseHit;
        result.damage = stats.damage;
        return result;
    }
}
