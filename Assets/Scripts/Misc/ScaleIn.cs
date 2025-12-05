using DG.Tweening;
using UnityEngine;

public class ScaleIn : MonoBehaviour
{
    public float scaleDuration;

    void Start()
    {
        Vector3 scale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, scaleDuration);
    }
}
