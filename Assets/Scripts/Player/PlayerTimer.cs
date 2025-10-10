using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    public float refillTime;
    public float remainingTime;

    void Awake()
    {
        remainingTime = refillTime;
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;

    }
}
