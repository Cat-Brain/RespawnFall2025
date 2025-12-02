public class DestroyOnTick : OnTickEffect
{
    public float duration;

    public override void OnTick(TickEntity tickEntity)
    {
        Destroy(gameObject, duration);
    }
}
