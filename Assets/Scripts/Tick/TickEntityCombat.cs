using UnityEngine;

public class TickEntityCombat : TickEntity
{
    public GameManager gameManager;

    [Tooltip("If true then only ticks if in combat and vice versa")]
    public bool shouldTickInCombat;

    public override void Tick()
    {
        if (gameManager.inCombat == shouldTickInCombat)
            base.Tick();
    }
}
