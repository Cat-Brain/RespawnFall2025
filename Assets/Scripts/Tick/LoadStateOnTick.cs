public class LoadStateOnTick : OnTickEffect
{
    public GameState state;
    public string sceneName;

    public override void OnTick(TickEntity tickEntity)
    {
        FindAnyObjectByType<GameManager>().LoadState(state, sceneName);
    }
}
