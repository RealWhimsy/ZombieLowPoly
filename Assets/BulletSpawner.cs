using UnityEngine;
using Random = UnityEngine.Random;

public class BulletSpawner : MonoBehaviour
{
    private GameObject player;
    private PlayerManager playerManager;
    private Weapon weapon;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        weapon = playerManager.GetActiveWeapon();
    }

    private void OnEnable()
    {
        EventManager.StartListening(Const.Events.SHOT_FIRED, Shoot);
        EventManager.StartListening(Const.Events.WEAPON_SWAPPED, HandleWeaponSwap);

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
    
    private void HandleWeaponSwap()
    {
        weapon = playerManager.GetActiveWeapon();
    }
}
