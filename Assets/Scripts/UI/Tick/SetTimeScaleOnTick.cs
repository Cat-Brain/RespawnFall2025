public class SetTimeScaleOnTick : OnTickEffect
{
    public float timeScale;

    public override void OnTick()
    {
        FindAnyObjectByType<GameManager>().SetTimeScale(timeScale);
    }
}
