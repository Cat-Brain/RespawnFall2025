using UnityEngine;

public class SetPlayerWeapon : OnTickEffect
{
    public CollisionTick collisionTick;

    public PlayerWeapon weapon;

    public override void OnTick(TickEntity tickEntity)
    {
        if (collisionTick.recentCollider.TryGetComponent(out PlayerWeaponInstance weapon))
            weapon.SetWeapon(this.weapon);
    }
}
