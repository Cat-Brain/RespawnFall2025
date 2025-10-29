public class StatusEffect
{
    public Status status;
    public Duration duration;

    public StatusEffect(HitStatus hitStatus)
    {
        status = hitStatus.status;
        duration = hitStatus.duration;

        // Add callback stuff here
    }

    public void ApplyDuration(Duration duration)
    {
        this.duration.ApplyDuration(duration);
    }
}
