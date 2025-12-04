using UnityEngine;

[CreateAssetMenu(fileName = "New TickIfAboveStackStatus", menuName = "StatusComponents/TickIfAboveStackStatus")]
public class TickIfAboveStackStatus : StatusComponent
{
    public int tickIndex;
    public int stacksRequired;

    protected StackStatus stackStatus;

    public override void Str(StatusEffect effect)
    {
        stackStatus = effect.GetComp<StackStatus>();
    }

    public override void Upd(StatusEffect effect)
    {
        if (stackStatus.stacks >= stacksRequired)
            effect.Tick(tickIndex);
    }
}
