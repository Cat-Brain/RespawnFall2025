using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "New DestroyOnDeath", menuName = "Health/DestroyOnDeath")]
public class DestroyOnDeath : Health
{
    public float deathTime;

    public override void OnDeath()
    {
        inst.transform.DOScale(0, deathTime).OnComplete(() => Destroy(inst.gameObject));
        inst.enabled = false;
    }
}
