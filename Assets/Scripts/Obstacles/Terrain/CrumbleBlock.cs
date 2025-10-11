using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CrumbleBlock : MonoBehaviour
{
    [Tooltip("Layers in this will cause the platform to crumble")]
    public LayerMask crumbleLayer;

    [Tooltip("Set to -1 to destroy on crumble")]
    public float disabledDuration;
    public float mercyTime, fadeDuration, fadeOpacity;

    private Collider2D col;
    private SpriteRenderer sr;

    private bool isCrumbled = false;
    private float mercyTimer = -1;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isCrumbled || mercyTimer == -1)
            return;

        mercyTimer = Mathf.Max(0, mercyTimer - Time.deltaTime);

        if (mercyTimer <= 0)
            StartCoroutine(Crumble());
    }

    void OnCollisionStay(Collision collision)
    {
        if (!isCrumbled && CMath.LayerOverlapsMask(collision.gameObject.layer, crumbleLayer))
            mercyTimer = -1;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!isCrumbled && CMath.LayerOverlapsMask(collision.gameObject.layer, crumbleLayer))
            mercyTimer = mercyTime;
    }

    public IEnumerator Crumble()
    {
        isCrumbled = true;
        col.enabled = false;
        if (disabledDuration == -1)
        {
            this.enabled = false;
            sr.DOFade(fadeOpacity, fadeDuration).OnComplete(() =>
                { Destroy(gameObject); });
            yield return null;
        }
        else
        {
            float unfadedOpacity = sr.color.a; 
            sr.DOFade(fadeOpacity, fadeDuration);
            yield return new WaitForSeconds(disabledDuration - fadeDuration);
            sr.DOFade(unfadedOpacity, fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
            col.enabled = true;
            isCrumbled = false;
            mercyTimer = -1;
        }
    }
}
