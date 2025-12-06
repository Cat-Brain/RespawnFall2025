using UnityEngine;

public class IncDifficultyTick : OnTickEffect
{
    public int increment;

    public override void OnTick(TickEntity tickEntity)
    {
        FindAnyObjectByType<GameManager>().IncDifficulty(increment);
    }
}
