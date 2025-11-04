using UnityEngine;

[CreateAssetMenu(fileName = "New Duration", menuName = "StatusComponents/Duration")]
public class Duration : StatusComponent
{
    public float duration;

    public override void Reapply(StatusEffect effect, StatusComponent newApplicant)
    {
        duration += ((Duration)newApplicant).duration;
    }

    public override void Upd(StatusEffect effect)
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            effect.Destruct();
    }
}
