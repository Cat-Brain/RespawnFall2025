using System.Collections.Generic;
using UnityEngine;

public class DisplayStringController : MonoBehaviour
{
    public GameObject displayStringPrefab;
    public List<DisplayString> displayStrings = new();

    public Vector2 firstStringPos, lastStringPos;

    public int maxStrings, currentStrings;

    void Update()
    {
        if (displayStrings.Count < maxStrings)
            for (int i = displayStrings.Count; i < maxStrings; i++)
                displayStrings.Add(Instantiate(displayStringPrefab, transform).GetComponent<DisplayString>());
        else if (displayStrings.Count > maxStrings)
            for (int i = displayStrings.Count - 1; i >= maxStrings; i--)
            {
                Destroy(displayStrings[i].gameObject);
                displayStrings.RemoveAt(i);
            }

        for (int i = 0; i < displayStrings.Count; i++)
        {
            displayStrings[i].transform.localPosition = Vector3.Lerp(firstStringPos, lastStringPos, (float)i / (maxStrings - 1));
            displayStrings[i].currentlyEnabled = i < currentStrings;
        }
    }
}
