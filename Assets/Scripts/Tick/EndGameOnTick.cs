using UnityEngine;

public class EndGameOnTick : OnTickEffect
{
    public GameManager manager;

    public override void OnTick(TickEntity tickEntity)
    {
        if (manager == null)
            manager = FindAnyObjectByType<GameManager>();

        manager.PlayerReset();
    }
}
