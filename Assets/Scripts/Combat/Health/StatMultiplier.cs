using UnityEngine;

[CreateAssetMenu(fileName = "New StatMultiplier", menuName = "Stats/StatMultiplier")]
public class StatMultiplier : StatModifier
{
    public float multiplier;

    public override void Modify(ref float stat)
    {
        stat *= multiplier;
    }
}
