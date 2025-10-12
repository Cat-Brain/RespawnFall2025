using UnityEngine;

public class UniqueName : MonoBehaviour
{
    void Awake()
    {
        foreach (UniqueName other in FindObjectsByType<UniqueName>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            if (this != other && gameObject.name == other.gameObject.name)
            {
                DestroyImmediate(gameObject);
                return;
            }
    }
}
