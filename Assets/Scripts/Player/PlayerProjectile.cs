using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 4.0f;
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.linearVelocity = mousePosition;
        transform.Translate(rb.linearVelocity * speed * Time.deltaTime);
    }
}
