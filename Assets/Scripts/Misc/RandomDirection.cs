using UnityEngine;

public class RandomDirection : MonoBehaviour
{
    void Start()
    {
        if (Random.Range(0, 2) == 0)
            transform.localScale = CMath.V3NPP(transform.localScale);
    }
}
