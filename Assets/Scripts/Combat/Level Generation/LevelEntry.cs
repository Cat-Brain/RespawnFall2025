using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelEntry : MonoBehaviour
{
    public Transform maskTransform;
    public SpriteRenderer holeSR;

    public float levelStartVelocity, levelTransitionTime, levelBufferTime,
        holeTweenTime, textFadeTime;
    public Ease levelTransitionTween, holeTween;
    public float openHoleScale, closedHoleScale, consumingHoleScale;

    public string sortingLayer;

    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public PlayerManager playerManager;

    [HideInInspector] public int level;
    [HideInInspector] public bool holeOpen = false, entryValid = false;
    [HideInInspector] public string baseText = "";

    public void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerManager = gameManager.playerManager;
        holeSR.color = gameManager.GetLevelEntryColor();
        StartCoroutine(BeginLevel());
    }

    public IEnumerator BeginLevel()
    {
        maskTransform.localScale = Vector3.zero;
        playerManager.rb.DOMove(transform.position, levelTransitionTime).SetEase(levelTransitionTween)
            .OnComplete(() => maskTransform.DOScale(consumingHoleScale, holeTweenTime).SetEase(holeTween));

        yield return new WaitForSeconds(levelTransitionTime + levelBufferTime);

        playerManager.active = true;
        playerManager.rb.linearVelocity = Vector2.up * levelStartVelocity;
        playerManager.SetSortingLayer();
        CloseHole();
        gameManager.LevelTransitionComplete();
    }

    public void CloseHole()
    {
        maskTransform.DOScale(0, holeTweenTime).SetEase(holeTween);
        holeOpen = false;
    }
}
