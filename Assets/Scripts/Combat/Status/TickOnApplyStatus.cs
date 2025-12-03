using UnityEngine;

[CreateAssetMenu(fileName = "New TickOnApplyStatus", menuName = "StatusComponents/TickOnApplyStatus")]
public class TickOnApplyStatus : StatusComponent
{
    public bool tickOnStart;
    public int tickIndex;

    public override void Str(StatusEffect effect)
    {
        if (tickOnStart)
            effect.Tick(tickIndex);
    }

    public override void Reapply(StatusEffect effect, StatusComponent newApplicant)
    {
        effect.Tick(tickIndex);
    }
}
