
using UnityEngine;

[CreateAssetMenu(fileName = "New DamageOnTickStatus", menuName = "StatusComponents/DamageOnTickStatus")]
public class DamageOnTickStatus : StatusComponent
{
    public float damage;

    public override void Tick(StatusEffect effect)
    {
        effect.health.ApplyHitDamage(damage);
    }
}
