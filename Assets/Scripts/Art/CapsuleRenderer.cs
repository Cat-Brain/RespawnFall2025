using DG.Tweening;
using UnityEngine;

[ExecuteInEditMode]
public class CapsuleRenderer : MonoBehaviour
{
    public LineRenderer lr;

    public Color color;

    public void Update()
    {
        if (!lr)
            return;

        lr.transform.localScale = CMath.Div3(Vector3.one, transform.localScale);
        Vector2 dim = transform.localScale;
        lr.widthMultiplier = dim.y;
        Vector3 pos = dim.y >= 2 * dim.x ? Vector3.zero : Vector3.right * (dim.x - dim.y * 0.5f);
        lr.SetPosition(0, -pos);
        lr.SetPosition(1, pos);

        lr.startColor = color;
        lr.endColor = color;
    }

    public Tween DOColor(Color color, float duration)
    {
        return DOTween.To(() => this.color, value => this.color = value, color, duration);
    }
}
