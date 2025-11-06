using UnityEngine;

[CreateAssetMenu(fileName = "New StackDamageOnTickStatus", menuName = "StatusComponents/StackDamageOnTickStatus")]
public class StackDamageOnTickStatus : StatusComponent
{
    public int damagePerStack;

    protected StackStatus stackStatus;

    public override void Str(StatusEffect effect)
    {
        stackStatus = effect.GetComponent<StackStatus>();
    }

    public override void Tick(StatusEffect effect)
    {
        effect.health.ApplyHitDamage(damagePerStack * stackStatus.stacks);
    }
}
