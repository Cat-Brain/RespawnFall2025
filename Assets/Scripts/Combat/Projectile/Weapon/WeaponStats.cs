using System;

[Serializable]
public struct WeaponStats
{
    public float damage, fireRate, magazineSize, reloadSpeed, range;

    public WeaponStats(float damage = 1, float fireRate = 1,
        float magazineSize = 1, float reloadSpeed = 1, float range = 1)
    {
        this.damage = damage;
        this.fireRate = fireRate;
        this.magazineSize = magazineSize;
        this.reloadSpeed = reloadSpeed;
        this.range = range;
    }

    public static readonly WeaponStats DEFAULT = new (1, 1, 1, 1, 1);
}
