using UnityEngine;

[CreateAssetMenu(fileName = "New DurationStatus", menuName = "StatusComponents/DurationStatus")]
public class DurationStatus : StatusComponent
{
    public enum StackMode
    {
        ADD, MAX
    }

    public float duration;
    public StackMode stackMode;

    public override void Reapply(StatusEffect effect, StatusComponent newApplicant)
    {
        DurationStatus newStack = (DurationStatus)newApplicant;

        switch (newStack.stackMode)
        {
            case StackMode.ADD:
                duration += newStack.duration;
                break;
            case StackMode.MAX:
                duration = Mathf.Max(duration, newStack.duration);
                break;
        }
    }

    public override void Upd(StatusEffect effect)
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            effect.Destruct();
    }
}
