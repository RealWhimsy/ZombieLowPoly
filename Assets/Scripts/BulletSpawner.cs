﻿using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

public class BulletSpawner : MonoBehaviour
{
    private GameObject player;
    private PlayerManager playerManager;
    private Weapon weapon;
    private GameObject grenade;
    public SprayBar sprayBar;
    private float spray;
    private float lastShot;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sprayBar = GameObject.Find("SprayBar").GetComponent<SprayBar>();
        playerManager = player.GetComponent<PlayerManager>();
        weapon = playerManager.GetActiveWeapon();
        spray = 0;
        lastShot = Time.time;
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

        if(1.0f < Time.time - lastShot && spray >= 0){
             spray -= 0.1f;
             sprayBar.SetSpray(spray);
        }
        
    }

    private void Shoot()
    {
        lastShot = Time.time;
        var randSpread = Random.Range(-spray, spray);
        var spread = Quaternion.Euler(0, 0 + randSpread, 0);
        Transform playerBullet = Instantiate(weapon.Bullet.transform, transform.position, transform.rotation * spread);
        Bullet bulletScript = playerBullet.GetComponent<Bullet>();
        bulletScript.setDamage(weapon.Damage);
        if(spray < weapon.MaxBulletSpread){
            spray += weapon.MaxBulletSpread / 15;
            sprayBar.SetSpray(spray);
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
