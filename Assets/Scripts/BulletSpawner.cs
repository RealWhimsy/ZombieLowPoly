using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

public class BulletSpawner : MonoBehaviour
{
    private GameObject player;
    private PlayerManager playerManager;
    private Weapon weapon;
    private GameObject grenade;
    private float spray;
    private float lastShot;

    private Transform playerBullet;
    private Bullet bulletScript;
    private float randSpread;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        }
        
    }

    private void Shoot()
    {
        if(weapon.WeaponType == WeaponType.Shotgun) {
            Shotgun();
        }
        else{
        lastShot = Time.time;
        randSpread = Random.Range(-spray, spray);
        var spread = Quaternion.Euler(0, 0 + randSpread, 0);
        Transform playerBullet = Instantiate(weapon.Bullet.transform, transform.position, transform.rotation * spread);
        Bullet bulletScript = playerBullet.GetComponent<Bullet>();
        bulletScript.setDamage(weapon.Damage);
        if(spray < weapon.MaxBulletSpread){
            spray += weapon.MaxBulletSpread / 15;
        }
    }
    }

    private void Shotgun(){

        for(int i=0; i< Const.Shotgun.ShotgunSplinters; i++){
        randSpread = Random.Range(-weapon.MaxBulletSpread, weapon.MaxBulletSpread);
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
