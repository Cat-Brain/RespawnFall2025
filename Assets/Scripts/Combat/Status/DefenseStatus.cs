using UnityEngine;

[CreateAssetMenu(fileName = "New DefenseStatus", menuName = "StatusComponents/DefenseStatus")]
public class DefenseStatus : StatusComponent, IOnHitStatus
{
    public float defense, defensePerStack;

    protected StackStatus stackStatus;

    public override void Str(StatusEffect effect)
    {
        stackStatus = effect.GetComponent<StackStatus>();
    }

    public void OnHit(StatusEffect effect, ref Hit hit)
    {
        if (hit.damage > 0)
            hit.damage = Mathf.Max(0, hit.damage - defense + defensePerStack * stackStatus.stacks);
    }
}
