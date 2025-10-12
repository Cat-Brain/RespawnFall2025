using UnityEngine;
using UnityEngine.UI;

public class DisplayString : MonoBehaviour
{
    public Image image;
    public Color enabledColor, disabledColor;

    [HideInInspector] public bool currentlyEnabled;

    void Update()
    {
        image.color = currentlyEnabled ? enabledColor : disabledColor;
    }
}
