using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class PlayerShoot : MonoBehaviour
{
    public InputActionReference clickAction;
    public PlayerManager playerManagerScript;
    public GameObject bulletPrefab;
    public Vector3 spawn;
    public int currentStrings, maxStrings;
    // Time references the max cool down time, and timer references the current time.
    public float coolDownTime, coolDownTimer;
    public float regenTime, regenTimer, regenConsecTime; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coolDownTimer = 0;
        regenTimer = 0;
        currentStrings = maxStrings;
        clickAction.action.started += Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        coolDownTimer -= Time.deltaTime;

        regenTimer -= Time.deltaTime;
        if (currentStrings < maxStrings && regenTimer <= 0)
        {
            regenTimer = regenConsecTime;
            currentStrings++;
         }
    }

    void Shoot(InputAction.CallbackContext context) {
        if(coolDownTimer <= 0 && currentStrings > 0) {
            currentStrings--;
            spawn = new Vector3(transform.position.x, transform.position.y, 0);
            GameObject playerBullet = Instantiate(bulletPrefab, spawn, transform.rotation);
            Destroy(playerBullet, 4.0f);
            coolDownTimer = coolDownTime;
            regenTimer = regenTime;
        }
    }
}
