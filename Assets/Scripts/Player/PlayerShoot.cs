using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;


public class PlayerShoot : MonoBehaviour
{
    public InputActionReference clickAction;
    public PlayerManager playerManagerScript;
    public GameObject projectilePrefab;
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
            currentStrings = maxStrings;
            playerManagerScript.gameManager.stringController.currentStrings = currentStrings;
            AudioManager.instance.PlaySoundFXClip(reloadClip, transform, 1.0f);
            coolDownTimer = coolDownTime;
        }
        if (clickAction.action.inProgress && coolDownTimer <= 0 && currentStrings > 0 && !playerManagerScript.Stunned())
        {
            AudioManager.instance.PlaySoundFXClip(shootClips[maxStrings - currentStrings], transform, 1.0f);
            currentStrings--;
            playerManagerScript.gameManager.stringController.currentStrings = currentStrings;

            Instantiate(projectilePrefab, transform.position, transform.rotation).GetComponent<ProjectileInst>().Init(
                ((Vector2)(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position)).normalized);

            coolDownTimer = coolDownTime;
            regenTimer = regenTime;
        }
    }
}
