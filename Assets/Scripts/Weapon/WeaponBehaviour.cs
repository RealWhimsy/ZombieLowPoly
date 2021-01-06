using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponBehaviour : MonoBehaviour
{
    private PlayerManager playerManager;
    private GameObject player;

    private AmmoUi ammoUi;

    private Weapon weapon;

    private float shotTime;

    private AudioSource weaponSoundSource;

    private void Start()
    {
        AddGunToPlayer();

        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        weapon = playerManager.GetActiveWeapon();
        weaponSoundSource = gameObject.AddComponent<AudioSource>();
        
        EventManager.StartListening(Const.Events.WEAPON_SWAPPED, HandleWeaponSwap);

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
            // TODO melee attack
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // TODO switch weapon
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
            if (weapon.ShotsInCurrentMag > 0 && Time.time - shotTime > weapon.ShotCooldown)
            {
                ammoUi.ReduceBulletUi();
                weapon.ShotsInCurrentMag--;
                Shoot();
                shotTime = Time.time;
            }
        }
    }

    private void HandleReload()
    {
        if (weapon.ShotsInCurrentMag != weapon.MaxMagazineSize)
        {
            // audioSource.clip = gunShotSoundsArray[1];
            // audioSource.PlayOneShot(audioSource.clip);
            if (weapon.Magazines > 0)
            {
                weapon.Magazines--;
                weapon.Reload();
                StartCoroutine(ammoUi.Reload(weapon.MaxMagazineSize));
            }
        }
    }

    private void HandlePickup()
    {
    }
    

    private void Shoot()
    {
        SoundManagerRework.Instance.PlayEffectOneShot(playerManager.GetActiveWeapon().ShotSound);
        SoundManagerRework.Instance.PlayEffectDelayed(playerManager.GetActiveWeapon().ShellSound, 0.4f);
        EventManager.TriggerEvent(Const.Events.SHOT_FIRED);
        AmmoTracker();
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