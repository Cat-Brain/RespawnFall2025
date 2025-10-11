using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class PlayerShoot : MonoBehaviour
{
    public InputActionReference clickAction;
    public GameObject bulletPrefab;
    public Vector3 spawn;
    public int strings;
    public float coolDownTime = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        strings = 4;
        clickAction.action.started += Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        coolDownTime -= Time.deltaTime;
    }

    void Shoot(InputAction.CallbackContext context) {
        if(coolDownTime <= 0) {
            strings--;
            spawn = new Vector3(transform.position.x, transform.position.y, 0);
            GameObject playerBullet = Instantiate(bulletPrefab, spawn, transform.rotation);
            Destroy(playerBullet, 4.0f);
            coolDownTime = 0.5f;
        }
    }
}
