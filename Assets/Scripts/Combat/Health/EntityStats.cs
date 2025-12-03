using System.Collections.Generic;
using UnityEngine;

public enum StatTarget
{
    SPEED
}

public class EntityStats : MonoBehaviour
{
    public float speed = 1, attackRate = 1, damage = 1, maxHealth = 1;

    public List<StatChange> speedMods = new(), attackRateMods = new(), damageMods = new(), maxHealthMods = new();

    public void LateUpdate()
    {
        speed = EvalStat(ref speedMods, 1);
        attackRate = EvalStat(ref attackRateMods, 1);
        damage = EvalStat(ref damageMods, 1);
        maxHealth = EvalStat(ref maxHealthMods, 1);
    }

    public float EvalStat(ref List<StatChange> mods, float baseValue)
    {
        mods.RemoveAll((stat) => stat.applier == null);

        float result = baseValue;
        mods.Sort();
        foreach (StatChange mod in mods)
            mod.modifier.Modify(ref result);

        return result;
    }

    public void ApplyMod(StatChange mod)
    {
        switch (mod.target)
        {
            case StatTarget.SPEED:
                speedMods.Add(mod); break;
            default:
                Debug.LogWarning("INVALID STAT CHANGE TARGET!!!"); break;
        }
    }
}
