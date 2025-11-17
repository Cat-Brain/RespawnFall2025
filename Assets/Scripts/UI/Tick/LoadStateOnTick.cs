public class LoadStateOnTick : OnTickEffect
{
    public GameState state;
    public string sceneName;

    public override void OnTick()
    {
        FindAnyObjectByType<GameManager>().LoadState(state, sceneName);
    }
}
