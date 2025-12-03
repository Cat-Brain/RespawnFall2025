using UnityEngine;

[CreateAssetMenu(fileName = "New DamageOnTickStatus", menuName = "StatusComponents/DamageOnTickStatus")]
public class DamageOnTickStatus : StatusComponent
{
    public int tickIndex;
    public float damage;

    public override void Tick(StatusEffect effect, int tickIndex)
    {
        if (this.tickIndex == tickIndex)
            effect.health.ApplyHitDamage(damage);
    }
}
