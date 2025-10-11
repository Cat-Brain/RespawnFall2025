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
        direction = (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - Camera.main.transform.position - transform.position).normalized;
        PlayerManager playerManagerScript = FindFirstObjectByType<PlayerManager>();
        playerManagerScript.ApplyRecoil(-direction, recoilForce);
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = direction * speed;
    }
}
