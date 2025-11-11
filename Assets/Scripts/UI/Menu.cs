using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Menu : MonoBehaviour
{
    public List<Button> buttons;
    public List<EventOnAction> eventOnActions;
    public List<Canvas> canvases;

    public GameState state;

    public bool active, onScreen;

    public void Update()
    {
        if (!Application.isPlaying)
            onScreen = active;

        foreach (Canvas canvas in canvases)
            if (canvas)
                canvas.enabled = onScreen;
    }

    public void SetActive(bool active)
    {
        this.active = active;

        foreach (Button button in buttons)
            button.interactable = active;
        foreach (EventOnAction eventOnAction in eventOnActions)
            eventOnAction.active = active;

        if (active)
            SetToOn();
        else
            SetToOff();
    }

    public virtual void SetToOn() { }

    public virtual void SetToOff() { }

    public Vector2 GetRatio()
    {
        return new Vector2(Camera.main.aspect, 1);
    }
}
