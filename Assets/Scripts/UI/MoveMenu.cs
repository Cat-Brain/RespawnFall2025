using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MoveMenu : Menu
{
    public List<RectTransform> rectTransforms;

    public Vector2 onPosition, offPosition;
    public float onOffMoveTime;
    public Ease onEase, offEase;

    public Tween currentMoveTween;

    new public void Update()
    {
        if (!onScreen && !active)
            foreach (RectTransform rectTransform in rectTransforms)
                rectTransform.anchoredPosition = offPosition;
        else if (!Application.isPlaying)
            foreach (RectTransform rectTransform in rectTransforms)
                rectTransform.anchoredPosition = active ? onPosition : offPosition;

        base.Update();
    }

    public override void SetToOn()
    {
        onScreen = true;
        if (!Application.isPlaying)
            return;

        currentMoveTween?.Kill();
        foreach (RectTransform rectTransform in rectTransforms)
            currentMoveTween = rectTransform.DOAnchorPos(onPosition * GetRatio(), onOffMoveTime).SetEase(onEase).
                SetUpdate(true);
    }

    public override void SetToOff()
    {
        if (!Application.isPlaying)
            return;

        currentMoveTween?.Kill();
        foreach (RectTransform rectTransform in rectTransforms)
            currentMoveTween = rectTransform.DOAnchorPos(offPosition * GetRatio(), onOffMoveTime).SetEase(offEase).
            SetUpdate(true).OnComplete(() => onScreen = false);
    }

    [ProButton]
    public void SetOnPositionToCurrent(int index)
    {
        onPosition = rectTransforms[index].anchoredPosition;
    }

    [ProButton]
    public void SetOffPositionToCurrent(int index)
    {
        offPosition = rectTransforms[index].anchoredPosition;
    }
}
