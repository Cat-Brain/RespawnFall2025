public class SetTimeScaleOnTick : OnTickEffect
{
    public float timeScale;

    public override void OnTick(TickEntity tickEntity)
    {
        FindAnyObjectByType<GameManager>().SetTimeScale(timeScale);
    }
}
