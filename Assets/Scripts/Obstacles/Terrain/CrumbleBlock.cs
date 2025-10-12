using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbleBlock : MonoBehaviour
{
    [Tooltip("Layers in this will cause the platform to crumble")]
    public LayerMask crumbleLayer;

    [Tooltip("Set to -1 to destroy on crumble")]
    public float disabledDuration;
    public float mercyTime, crumbleTime, fadeDuration, fadeOpacity;

    private Collider2D col;
    private SpriteRenderer sr;

    private bool isCrumbled = false;
    private float mercyTimer = -1;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (isCrumbled)
            return;

        List<Collider2D> overlaps = new();
        col.Overlap(overlaps);
        foreach (Collider2D overlap in overlaps)
            if (!isCrumbled && CMath.LayerOverlapsMask(overlap.gameObject.layer, crumbleLayer))
            {
                mercyTimer = Mathf.Max(0, mercyTimer - Time.fixedDeltaTime);
                if (mercyTimer > 0)
                    return;

                isCrumbled = true;
                StartCoroutine(Crumble());
                return;
            }

        mercyTimer = mercyTime;
    }

    public IEnumerator Crumble()
    {
        isCrumbled = true;
        yield return new WaitForSeconds(crumbleTime);
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
            mercyTimer = mercyTime;
            col.enabled = true;
            isCrumbled = false;
        }
    }
}
