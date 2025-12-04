using System;
using UnityEngine;

public abstract class StatModifier : ScriptableObject, IComparable<StatModifier>
{
    public int order;

    public abstract void Modify(ref float stat);

    public int CompareTo(StatModifier other)
    {
        return order.CompareTo(other.order);
    }
}
