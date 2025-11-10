using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponInstance : MonoBehaviour
{
    public InputActionReference primaryAction, reloadAction;
    public PlayerManager playerManager;

    public PlayerWeapon weapon;

    void Awake()
    {
        weapon = (PlayerWeapon)weapon.Copy(transform);
    }

    void Update()
    {
        if (playerManager.Stunned())
            return;

        weapon.reload.Upd(reloadAction.action.inProgress);
        if (!weapon.reload.reloading)
            weapon.primary.Upd(primaryAction.action.inProgress);
    }
}