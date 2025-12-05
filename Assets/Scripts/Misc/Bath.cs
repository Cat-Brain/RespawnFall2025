using UnityEngine;

public class Bath : MonoBehaviour
{
    public string desiredTag;
    public float healPerTick, timePerHeal;
    public int totalHeals;

    public float lastHeal = -100;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (totalHeals == 0 || Time.time - lastHeal < timePerHeal)
            return;

        if (collision.CompareTag(desiredTag) && collision.TryGetComponent(out HealthInst health))
        {
            health.ApplyHitDamage(-healPerTick);
            lastHeal = Time.time;
            if (totalHeals > 0)
                totalHeals--;
        }
    }
}
