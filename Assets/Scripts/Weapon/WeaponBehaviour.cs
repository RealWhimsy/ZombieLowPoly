using System.Collections;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    private PlayerManager playerManager;
    private GameObject player;
    private GameObject meleeZone;

    private AmmoUi ammoUi;

    private Weapon weapon;

    private float shotTime;
    private static readonly int ShootAnimation = Animator.StringToHash("shoot");
    private static readonly int ReloadAnimation = Animator.StringToHash("reload");

    private void Start()
    {
        AddGunToPlayer();

        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        meleeZone = player.transform.Find("MeleeArea").gameObject;
        weapon = playerManager.GetActiveWeapon();

        EventManager.StartListening(Const.Events.WeaponSwapped, HandleWeaponSwap);

        shotTime = Time.time;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleLeftClick();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            HandleReload();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            MeleeAttack();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGranade();
        }
    }

    private void AddGunToPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        ammoUi = player.AddComponent<AmmoUi>();
    }

    private void HandleWeaponSwap()
    {
        weapon = playerManager.GetActiveWeapon();
    }

    private void HandleLeftClick()
    {
        if (!ammoUi.reloading)
        {
            if (weapon.ShotsInCurrentMag > 0 && Time.time - shotTime > weapon.ShotCooldown && !weapon.MeleeWeapon)
            {
                ammoUi.ReduceBulletUi();
                weapon.ShotsInCurrentMag--;
                playerManager.anim.SetTrigger(ShootAnimation);
                Shoot();
                
                shotTime = Time.time;
            }
            if(weapon.MeleeWeapon)
            {
                playerManager.anim.SetTrigger(ShootAnimation);
            }
        }
    }

    private void HandleReload()
    {
        if (weapon.ShotsInCurrentMag != weapon.MaxMagazineSize)
        {
            if (weapon.Magazines > 0)
            {
                weapon.Magazines--;
                playerManager.anim.SetTrigger(ReloadAnimation);
                weapon.Reload();
                SoundManagerRework.Instance.PlayEffectOneShot(weapon.ReloadSound);
                StartCoroutine(ammoUi.Reload(weapon.MaxMagazineSize));
            }
        }
    }
    
    private void Shoot()
    {
        SoundManagerRework.Instance.PlayEffectOneShot(weapon.ShotSound);
        SoundManagerRework.Instance.PlayEffectDelayed(weapon.ShellSound, 0.4f);
        EventManager.TriggerEvent(Const.Events.ShotFired);
        AmmoTracker();
    }

    private void MeleeAttack()
    {
        meleeZone.SetActive(true);
        StartCoroutine(waitAndDisableMeleeZone());
        EventManager.TriggerEvent(Const.Events.MeleeAttack);
    }

    private void ThrowGranade(){
        if(playerManager.granades != 0){
            playerManager.granades--;
            ammoUi.ReduceGranadeUi();
            EventManager.TriggerEvent(Const.Events.GranadeThrown);
        }
    }

    IEnumerator waitAndDisableMeleeZone()
    {
        yield return new WaitForSeconds(1);
        meleeZone.SetActive(false);
    }
    
    private void AmmoTracker()
    {
        if (weapon.ShotsInCurrentMag <= 0 && weapon.Magazines <= 0)
        {
            ammoUi.OutOfAmmoText();
        }

        if (weapon.ShotsInCurrentMag <= 0 && weapon.Magazines > 0)
        {
            ammoUi.ReloadText();
        }
    }
}