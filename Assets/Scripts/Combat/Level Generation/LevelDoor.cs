using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelDoor : OnTickEffect
{
    public TextMeshProUGUI text;
    public Transform maskTransform;
    public Collider2D col;

    public bool beginOnAwake;
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
    public static string playerSortingLayer = "";

    public void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerManager = gameManager.playerManager;
        //level = gameManager.level;
        text.color = new Color(1, 1, 1, 0);
        UpdateText();
        if (beginOnAwake)
            StartCoroutine(BeginLevel());
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

    public IEnumerator BeginLevel()
    {
        maskTransform.localScale = Vector3.zero;
        playerManager.rb.DOMove(transform.position, levelTransitionTime).SetEase(levelTransitionTween)
            .OnComplete(() => maskTransform.DOScale(consumingHoleScale, holeTweenTime).SetEase(holeTween));

        yield return new WaitForSeconds(levelTransitionTime + levelBufferTime);

        playerManager.active = true;
        playerManager.rb.linearVelocity = Vector2.up * levelStartVelocity;
        playerManager.SetSortingLayer(playerManager.sortingLayer);
        CloseHole();
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
        //gameManager.LoadNextLevel();
    }

    public void UpdateText()
    {
        if (baseText == "")
            baseText = text.text;
        text.text = baseText + (level + 1).ToString();
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
