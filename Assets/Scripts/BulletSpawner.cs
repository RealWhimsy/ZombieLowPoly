﻿using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

public class BulletSpawner : MonoBehaviour
{
    private GameObject player;
    private PlayerManager playerManager;
    private Weapon weapon;
    private GameObject grenade;
    private Transform playerBullet;
    private Bullet bulletScript;
    private float randSpread;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        weapon = playerManager.GetActiveWeapon();

    }

    private void OnEnable()
    {
        EventManager.StartListening(Const.Events.ShotFired, Shoot);
        EventManager.StartListening(Const.Events.GrenadeThrown, Throw);
        EventManager.StartListening(Const.Events.WeaponSwapped, HandleWeaponSwap);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Shoot()
    {
        if(weapon.WeaponType == WeaponType.Shotgun) {
            Shotgun();
        }
        else{
            randSpread = Random.Range(-weapon.BulletSpread, weapon.BulletSpread);
            var spread = Quaternion.Euler(0, 0 + randSpread, 0);
            Transform playerBullet = Instantiate(weapon.Bullet.transform, transform.position, transform.rotation * spread);
            Bullet bulletScript = playerBullet.GetComponent<Bullet>();
            bulletScript.setDamage(weapon.Damage);
        }
    }

    private void Shotgun(){

        for(int i=0; i< Const.Shotgun.ShotgunSplinters; i++){
        randSpread = Random.Range(-weapon.BulletSpread, weapon.BulletSpread);
        playerBullet = Instantiate(weapon.Bullet.transform, transform.position, transform.rotation * Quaternion.Euler(0, randSpread, 0));
        bulletScript = playerBullet.GetComponent<Bullet>();
        bulletScript.setDamage(weapon.Damage / Const.Shotgun.ShotgunSplinters);
        }
    }

    private IEnumerator ThrowGrenade()
    {
        yield return new WaitForSeconds(0.5f);
        grenade = (GameObject) Resources.Load(Const.Grenade.GrenadePrefab, typeof(GameObject));
        Transform grenadePrefab = Instantiate(grenade.transform, transform.position, transform.rotation);
        Grenade grenadeScript = grenadePrefab.GetComponent<Grenade>();
        grenadeScript.setDamage(Const.Grenade.GrenadeDamage);
    }

    private void Throw(){
        StartCoroutine(ThrowGrenade());
    }
    
    private void HandleWeaponSwap()
    {
        weapon = playerManager.GetActiveWeapon();
    }
}
