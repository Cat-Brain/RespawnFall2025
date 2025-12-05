using UnityEngine;

[CreateAssetMenu(fileName = "New StackDamageOnTickStatus", menuName = "StatusComponents/StackDamageOnTickStatus")]
public class StackDamageOnTickStatus : StatusComponent
{
    public int tickIndex;
    public float damagePerStack;

    protected StackStatus stackStatus;

    public override void Str(StatusEffect effect)
    {
        stackStatus = effect.GetComp<StackStatus>();
    }

    public override void Tick(StatusEffect effect, int tickIndex)
    {
        if (this.tickIndex == tickIndex)
            effect.health.ApplyHitDamage(damagePerStack * stackStatus.stacks);
    }
}
