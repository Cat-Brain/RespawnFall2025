using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelWin : OnTickEffect
{
    public TextMeshProUGUI text;
    public Transform maskTransform;
    public SpriteRenderer holeSR;
    public Collider2D col;

    public float holeTweenTime, textFadeTime;
    public Ease holeTween;
    public float openHoleScale, closedHoleScale, consumingHoleScale;

    public string sortingLayer;

    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public PlayerManager playerManager;

    [HideInInspector] public bool holeOpen = false, entryValid = false;
    [HideInInspector] public string baseText = "";

    public void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerManager = gameManager.playerManager;
        text.color = new Color(1, 1, 1, 0);
        maskTransform.localScale = Vector3.zero;
    }

    public void LateUpdate()
    {
        if (!gameManager.inCombat && gameManager.generationManager.enemySpawnManager.inBoss && !holeOpen)
            OpenHole();

        if (holeOpen && !entryValid && playerManager.col.IsTouching(col))
            EnableEntry();
        else if (entryValid && !playerManager.col.IsTouching(col))
            DisableEntry();
    }

    public override void OnTick(TickEntity tickEntity)
    {
        if (entryValid && gameManager.CanExitLevel())
            StartCoroutine(ExitLevel());
    }

    public IEnumerator ExitLevel()
    {
        entryValid = false;
        maskTransform.DOScale(consumingHoleScale, holeTweenTime).SetEase(holeTween);

        yield return new WaitForSeconds(holeTweenTime);

        CloseHole();
        gameManager.PlayerWin();
    }

    public void CloseHole()
    {
        maskTransform.DOScale(0, holeTweenTime).SetEase(holeTween);
        holeOpen = false;
        text.DOFade(0, textFadeTime);
        entryValid = false;
    }

    public void OpenHole()
    {
        maskTransform.DOScale(closedHoleScale, holeTweenTime).SetEase(holeTween);
        holeOpen = true;
    }

    public void EnableEntry()
    {
        maskTransform.DOScale(openHoleScale, holeTweenTime).SetEase(holeTween);
        text.DOFade(1, textFadeTime);
        entryValid = true;
    }

    public void DisableEntry()
    {
        maskTransform.DOScale(closedHoleScale, holeTweenTime).SetEase(holeTween);
        text.DOFade(0, textFadeTime);
        entryValid = false;
    }
}
