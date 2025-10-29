public abstract class Duration
{
    // Returns true if this StatusEffect should be removed
    public abstract bool Update(StatusEffect statusEffect);
    public abstract void ApplyDuration(Duration duration);
}
