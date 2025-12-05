using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;    

[ExecuteInEditMode]
public class PlayerWeaponInstance : MonoBehaviour
{
    public EntityStat damageStat, fireRateStat, magazineSizeStat, reloadSpeedStat, rangeStat;
    public InputActionReference primaryAction, reloadAction;
    public PlayerManager playerManager;

    public PlayerWeapon weapon;
      
    // Audio Stuff
    private FMOD.Studio.EventInstance instance;

    void Awake()
    {
        if (Application.isPlaying)
            SetWeapon(weapon);
    }

    void Start()
    {
        instance = GetComponent<StudioEventEmitter>().EventInstance;
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

        weapon.stats.damage = damageStat.value;
        weapon.stats.fireRate = fireRateStat.value;
        weapon.stats.magazineSize = magazineSizeStat.value;
        weapon.stats.reloadSpeed = reloadSpeedStat.value;
        weapon.stats.range = rangeStat.value;

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
        damageStat.baseValue = weapon.baseHit.damage;
    }
}