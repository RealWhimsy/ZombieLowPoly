using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponBehaviour : MonoBehaviour
{
    private UnityAction leftClickListener;
    private UnityAction reloadListener;
    private UnityAction pickupListener;

    private PlayerManager playerManager;
    private GameObject player;

    private Gun weaponLogic;

    private Weapon weapon;

    private float shotTime;

    private void Start()
    {
        leftClickListener = new UnityAction(HandleLeftClick);
        reloadListener = new UnityAction(HandleReload);
        pickupListener = new UnityAction(HandlePickup);

        AddGunToPlayer();

        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        weapon = playerManager.GetActiveWeapon();
        
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
        weaponLogic = player.AddComponent<Gun>();
    }

    private void HandleWeaponSwap()
    {
        weapon = playerManager.GetActiveWeapon();
    }

    private void HandleLeftClick()
    {
        if (!weaponLogic.reloading)
        {
            if (weapon.ShotsInCurrentMag > 0 && Time.time - shotTime > weapon.ShotCooldown)
            {
                weaponLogic.ReduceBulletUi();
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
                StartCoroutine(weaponLogic.Reload(weapon.MaxMagazineSize));
            }
        }
    }

    private void HandlePickup()
    {
    }
    

    private void Shoot()
    {
        // if (melee)
        // {
        //     meleeAttack();
        // }

        // audioSource.clip = gunShotSoundsArray[0];
        // audioSource.PlayOneShot(audioSource.clip);
        // audioSource.clip = gunShotSoundsArray[2];
        // audioSource.PlayDelayed((float) 0.4);
        
        EventManager.TriggerEvent(Const.Events.SHOT_FIRED);
        AmmoTracker();
    }
    
    private void AmmoTracker()
    {
        if (weapon.ShotsInCurrentMag <= 0 && weapon.Magazines <= 0)
        {
            weaponLogic.OutOfAmmoText();
        }

        if (weapon.ShotsInCurrentMag <= 0 && weapon.Magazines > 0)
        {
            weaponLogic.ReloadText();
        }
    }
}