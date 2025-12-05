using TMPro;

public class IncPriceTick : OnTickEffect
{
    public TextMeshProUGUI text;
    public int increment;

    public override void OnTick(TickEntity tickEntity)
    {
        ((PriceTick)tickEntity).cost += increment;
        text.text = ((PriceTick)tickEntity).cost.ToString();
    }
}
