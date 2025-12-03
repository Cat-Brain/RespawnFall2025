using UnityEngine;

public class CloseGameOnTick : OnTickEffect
{
    public override void OnTick(TickEntity tickEntity)
    {
        Application.Quit();
    }
}
