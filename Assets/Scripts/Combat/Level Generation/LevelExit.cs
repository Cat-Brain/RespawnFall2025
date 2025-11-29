using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelExit : OnTickEffect
{
    public TextMeshProUGUI text;
    public Transform maskTransform;
    public SpriteRenderer holeSR;
    public Collider2D col;

    public int pathIndex;
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
        UpdateText();
        holeSR.color = gameManager.GetLevelExitColor(pathIndex);
        maskTransform.localScale = Vector3.zero;
    }

    public void LateUpdate()
    {
        if (!gameManager.inCombat && !holeOpen)
            OpenHole();

        if (holeOpen && !entryValid && playerManager.col.IsTouching(col))
            EnableEntry();
        else if (entryValid && !playerManager.col.IsTouching(col))
            DisableEntry();
    }

    public override void OnTick(TickEntity tickEntity)
    {
        if (entryValid)
            StartCoroutine(ExitLevel());
    }

    public IEnumerator ExitLevel()
    {
        entryValid = false;
        maskTransform.DOScale(consumingHoleScale, holeTweenTime).SetEase(holeTween);

        playerManager.active = false;
        playerManager.rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(holeTweenTime);

        playerManager.SetSortingLayer(sortingLayer);

        CloseHole();
        gameManager.LoadNextLevel(pathIndex);
    }

    public void UpdateText()
    {
        text.color = new Color(1, 1, 1, 0);
        if (baseText == "")
            baseText = text.text;
        text.text = baseText + gameManager.GetLevelExitText(pathIndex);
    }

    public void CloseHole()
    {
        maskTransform.DOScale(0, holeTweenTime).SetEase(holeTween);
        holeOpen = false;
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
