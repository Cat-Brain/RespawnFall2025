using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PriceTick : TickWhenHoveredEntity
{
    public int cost;
    public TextMeshProUGUI text;

    [HideInInspector] public GameManager manager;

    public override void Tick()
    {
        if (manager == null)
            manager = FindAnyObjectByType<GameManager>();

        if (manager.currentLevelMoney < cost ||
            !col.OverlapPoint(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue())))
            return;

        manager.currentLevelMoney -= cost;
        base.Tick();
    }

    public override void BypassTick()
    {
        if (manager == null)
            manager = FindAnyObjectByType<GameManager>();

        if (manager.currentLevelMoney < cost)
            return;

        manager.currentLevelMoney -= cost;
        base.BypassTick();
    }

    void LateUpdate()
    {
        text.text = cost.ToString();
    }
}
