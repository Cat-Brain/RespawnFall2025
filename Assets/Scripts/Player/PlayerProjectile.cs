using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerProjectile : MonoBehaviour
{
    public Vector3 direction;
    public float recoilForce;
    public float speed;
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        direction = ((Vector2)(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position)).normalized;
        PlayerManager playerManagerScript = FindFirstObjectByType<PlayerManager>();
        playerManagerScript.ApplyRecoil(-direction, recoilForce);
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 0)
        {
            Destroy(gameObject);
        }
    }

}
