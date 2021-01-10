using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

public class BulletSpawner : MonoBehaviour
{
    private GameObject player;
    private PlayerManager playerManager;
    private Weapon weapon;
    private GameObject granade;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        weapon = playerManager.GetActiveWeapon();
    }

    private void OnEnable()
    {
        EventManager.StartListening(Const.Events.ShotFired, Shoot);
        EventManager.StartListening(Const.Events.GranadeThrown, Throw);
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

    private IEnumerator ThrowGranade()
    {
        yield return new WaitForSeconds(0.5f);
        granade = (GameObject) Resources.Load("Prefabs/Granade", typeof(GameObject));
        Transform granadePrefab = Instantiate(granade.transform, transform.position, transform.rotation);
        Granade granadeScript = granadePrefab.GetComponent<Granade>();
        granadeScript.setDamage(100);
    }

    private void Throw(){
        StartCoroutine(ThrowGranade());
    }
    
    private void HandleWeaponSwap()
    {
        weapon = playerManager.GetActiveWeapon();
    }
}
