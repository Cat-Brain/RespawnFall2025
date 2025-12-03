using UnityEngine;

[CreateAssetMenu(fileName = "New TickOnApplyStatus", menuName = "StatusComponents/TickOnApplyStatus")]
public class TickOnApplyStatus : StatusComponent
{
    public int tickIndex;

    public override void Reapply(StatusEffect effect, StatusComponent newApplicant)
    {
        effect.Tick(tickIndex);
    }
}
