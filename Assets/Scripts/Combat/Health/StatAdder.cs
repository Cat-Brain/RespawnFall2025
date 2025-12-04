using UnityEngine;

[CreateAssetMenu(fileName = "New StatAdder", menuName = "Stats/StatAdder")]
public class StatAdder : StatModifier
{
    public float addition;

    public override void Modify(ref float stat)
    {
        stat += addition;
    }
}
