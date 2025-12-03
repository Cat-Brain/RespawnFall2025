using UnityEngine;

[CreateAssetMenu(fileName = "New StackStatus", menuName = "StatusComponents/StackStatus")]
public class StackStatus : StatusComponent
{
    public int stacks;

    public override void Reapply(StatusEffect effect, StatusComponent newApplicant)
    {
        stacks += ((StackStatus)newApplicant).stacks;
    }

    // Returns true if this operation caused the stacks to become <= 0 and therefore to destruct
    public bool ModifyStack(StatusEffect effect, int quantity)
    {
        if (stacks <= -quantity)
        {
            stacks = 0;
            effect.Destruct();
            return true;
        }
        stacks += quantity;
        return false;
    }
}
