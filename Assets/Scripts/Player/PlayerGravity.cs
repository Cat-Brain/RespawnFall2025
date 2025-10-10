using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    public float gravity;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocityY -= gravity * Time.fixedDeltaTime;
    }
}
