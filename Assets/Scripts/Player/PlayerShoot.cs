using UnityEngine;
using System.Collections;


public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Vector3 spawn;
    public int strings;
    public float coolDownTime = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        strings = 4;
    }

    // Update is called once per frame
    void Update()
    {
        coolDownTime -= Time.deltaTime;
        if(Input.GetMouseButton(0) && coolDownTime <= 0f) {
            Shoot();
            coolDownTime = 0.5f;
        }
    }

    void Shoot() {
        strings--;
        spawn = new Vector3(transform.position.x, transform.position.y, 0);
        GameObject playerBullet = Instantiate(bulletPrefab, spawn, transform.rotation);
        Destroy(playerBullet, 4.0f);
    }
}
