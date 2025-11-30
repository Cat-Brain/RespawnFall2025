using System.Collections;
using UnityEngine;

public class EnableWithDelay : MonoBehaviour
{
    public void Init(GameObject toEnable, float duration)
    {
        StartCoroutine(InternalInit(toEnable, duration));
    }

    public IEnumerator InternalInit(GameObject toEnable, float duration)
    {
        yield return new WaitForSeconds(duration);
        toEnable.SetActive(true);
    }
}
