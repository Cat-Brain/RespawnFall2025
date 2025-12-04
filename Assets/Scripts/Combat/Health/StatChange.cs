using UnityEngine;

public class StatChange : System.IComparable<StatChange>
{
    public StatModifier modifier;
    public Object applier;
    public StatTarget target;

    public StatChange(StatModifier modifier, Object applier, StatTarget target)
    {
        this.modifier = modifier;
        this.applier = applier;
        this.target = target;
    }

    public int CompareTo(StatChange other)
    {
        return modifier.CompareTo(other.modifier);
    }
}