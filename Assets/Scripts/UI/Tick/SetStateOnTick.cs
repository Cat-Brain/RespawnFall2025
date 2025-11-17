public class SetStateOnTick : OnTickEffect
{
    public GameState state;

    public override void OnTick()
    {
        FindAnyObjectByType<GameManager>().SetState(state);
    }
}
