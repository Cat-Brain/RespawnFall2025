using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class PlayerWeaponInstance : MonoBehaviour
{
    public EntityStat damageStat;
    public InputActionReference primaryAction, reloadAction;
    public PlayerManager playerManager;

    public PlayerWeapon weapon;

    void Awake()
    {
        if (Application.isPlaying)
            SetWeapon(weapon);
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            playerManager.SetMainColor(weapon.playerMainColor, weapon.playerMainColor);
            playerManager.SetBeakColor(weapon.playerBeakColor, weapon.playerBeakColor);
            playerManager.SetEyeColor(weapon.playerEyeColor, weapon.playerEyeColor);
            return;
        }

        if (playerManager.Stunned())
            return;

        weapon.reload.Upd(reloadAction.action.inProgress);
        if (!weapon.reload.reloading)
            weapon.primary.Upd(primaryAction.action.inProgress);
    }

    public void SetWeapon(PlayerWeapon weapon)
    {
        playerManager.SetMainColor(weapon.playerMainColor, this.weapon.playerMainColor);
        playerManager.SetBeakColor(weapon.playerBeakColor, this.weapon.playerBeakColor);
        playerManager.SetEyeColor(weapon.playerEyeColor, this.weapon.playerEyeColor);
        this.weapon = (PlayerWeapon)weapon.Copy(transform);
    }
}