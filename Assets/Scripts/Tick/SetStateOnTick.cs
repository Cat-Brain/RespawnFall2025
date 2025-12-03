public class SetStateOnTick : OnTickEffect
{
    public GameState state;

    public override void OnTick(TickEntity tickEntity)
    {
        FindAnyObjectByType<GameManager>().SetState(state);
    }
}
