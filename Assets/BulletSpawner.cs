using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

public class BulletSpawner : MonoBehaviour
{
    private GameObject player;
    private PlayerManager playerManager;
    private Weapon weapon;
    private GameObject grenade;
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
        var randSpread = Random.Range(-weapon.BulletSpread, weapon.BulletSpread);
        var spread = Quaternion.Euler(0, 0 + randSpread, 0);
        Transform playerBullet = Instantiate(weapon.Bullet.transform, transform.position, transform.rotation * spread);
        Bullet bulletScript = playerBullet.GetComponent<Bullet>();
        bulletScript.setDamage(weapon.Damage);
    }

    private IEnumerator ThrowGrenade()
    {
        yield return new WaitForSeconds(0.5f);
        grenade = (GameObject) Resources.Load("Prefabs/Grenade", typeof(GameObject));
        Transform grenadePrefab = Instantiate(grenade.transform, transform.position, transform.rotation);
        Grenade grenadeScript = grenadePrefab.GetComponent<Grenade>();
        grenadeScript.setDamage(Const.Granade.GranadeDamage);
    }

    private void Throw(){
        StartCoroutine(ThrowGrenade());
    }
    
    private void HandleWeaponSwap()
    {
        weapon = playerManager.GetActiveWeapon();
    }
}
