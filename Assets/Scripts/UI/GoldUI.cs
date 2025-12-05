using DG.Tweening;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    public GameManager gameManager;
    public RectTransform mainTransform;
    public TextMeshProUGUI text;

    public float enabledScale, disabledScale, scaleTime;
    public bool currentlyEnabled = true;

    void Start()
    {
        currentlyEnabled = ShouldEnable();
        mainTransform.localScale = Vector3.one * DesiredScale();
    }

    void Update()
    {
        if (ShouldEnable())
            Enable();
        else
            Disable();

        text.text = gameManager.currentLevelMoney.ToString();
    }

    public bool ShouldEnable()
    {
        return gameManager.currentLevelMoney > 0;
    }

    public float DesiredScale()
    {
        return currentlyEnabled ? enabledScale : disabledScale;
    }

    public void Disable()
    {
        if (!currentlyEnabled)
            return;
        currentlyEnabled = false;

        mainTransform.DOScale(disabledScale, scaleTime);
    }

    public void Enable()
    {
        if (currentlyEnabled)
            return;
        currentlyEnabled = true;

        mainTransform.DOScale(enabledScale, scaleTime);
    }
}
