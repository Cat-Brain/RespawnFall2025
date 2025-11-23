using UnityEngine;

[ExecuteInEditMode]
public class CapsuleController : MonoBehaviour
{
    public CapsuleRenderer capsuleRenderer;

    public SpriteRenderer frontCircle, backCircle, middleSquare;

    void LateUpdate()
    {
        if (capsuleRenderer == null && !transform.parent.TryGetComponent(out capsuleRenderer))
            return;

        transform.localScale = CMath.Div3(Vector3.one, transform.parent.localScale);

        Vector2 dim = transform.parent.localScale;
        frontCircle.transform.localPosition = Vector2.right * (dim.x - dim.y * 0.5f);
        backCircle.transform.localPosition = -frontCircle.transform.localPosition;
        frontCircle.transform.localScale = Vector3.one * dim.y;
        backCircle.transform.localScale = frontCircle.transform.localScale;
        middleSquare.transform.localScale = new Vector3(
            frontCircle.transform.localPosition.x * 2, dim.y, 1);

        frontCircle.color = capsuleRenderer.color;
        backCircle.color = capsuleRenderer.color;
        middleSquare.color = capsuleRenderer.color;

        //frontCircle.sortingLayerID
    }
}
