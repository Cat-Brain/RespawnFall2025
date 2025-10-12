using DG.Tweening;
using UnityEngine;

public class TimerRefresher : MonoBehaviour
{
    public SpriteRenderer sr;
    public string playerTag;

    public float fadeTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled && collision.CompareTag(playerTag) && collision.TryGetComponent(out PlayerManager playerManager))
        {
            playerManager.playerTimer.RefreshTime();
            enabled = false;
            sr.DOFade(0, fadeTime).OnComplete(() => { Destroy(gameObject); });
        }
    }
}
