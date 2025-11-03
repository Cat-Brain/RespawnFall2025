using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;


public class PlayerShoot : MonoBehaviour
{
    public InputActionReference clickAction;
    public PlayerManager playerManagerScript;
    public GameObject bulletPrefab;
    public Vector3 spawn;
    public int currentStrings, maxStrings;
    public bool hasStrings, hasExplosives, hasPhasing, hasRecoil; // Orpheus upgrades
    public AudioClip[] shootClips;
    public AudioClip reloadClip;
    
    // Time references the max cool down time, and timer references the current time.
    public float coolDownTime, coolDownTimer;
    public float regenTime, regenTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coolDownTimer = 0;
        regenTimer = 0;
        currentStrings = maxStrings;
        playerManagerScript.gameManager.stringController.maxStrings = maxStrings;
        playerManagerScript.gameManager.stringController.currentStrings = currentStrings;
    }

    // Update is called once per frame
    void Update()
    {
        coolDownTimer -= Time.deltaTime;

        regenTimer -= Time.deltaTime;
        if (currentStrings < maxStrings && regenTimer <= 0)
        {
            currentStrings++;
            playerManagerScript.gameManager.stringController.currentStrings = currentStrings;

            if (currentStrings == maxStrings - 1)
            {
                AudioManager.instance.PlaySoundFXClip(reloadClip, transform, 1.0f);
            }
        }
        if (clickAction.action.inProgress && coolDownTimer <= 0 && currentStrings > 0 && !playerManagerScript.Stunned())
        {
            AudioManager.instance.PlaySoundFXClip(shootClips[maxStrings - currentStrings], transform, 1.0f);
            currentStrings--;
            playerManagerScript.gameManager.stringController.currentStrings = currentStrings;
            spawn = new Vector3(transform.position.x, transform.position.y, 0);
            GameObject playerBullet = Instantiate(bulletPrefab, spawn, transform.rotation);
            coolDownTimer = coolDownTime;
            regenTimer = regenTime;
        }
    }
}
