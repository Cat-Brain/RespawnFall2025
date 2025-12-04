using System.Collections.Generic;
using UnityEngine;

public enum StatTarget
{
    SPEED, DAMAGE, ATTACK_RATE, MAX_HEALTH, // Common
    JUMP_HEIGHT,
    FIRE_RATE, MAGAZINE_SIZE, RELOAD_SPEED, RANGE, // Projectile Stuff
}

public class EntityStat : MonoBehaviour
{
    public StatTarget target;
    public float baseValue = 1;
    
    public float value;
    public List<StatChange> mods = new();

    public void LateUpdate()
    {
        mods.RemoveAll((stat) => stat.applier == null);

        value = baseValue;
        mods.Sort();
        foreach (StatChange mod in mods)
            mod.modifier.Modify(ref value);
    }
}
