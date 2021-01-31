using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IDamageDealer
{
    public float bulletSpeed;
    public int bulletDistance;
    int damage;

    private Vector3 playerPosition;
    private GameObject player;

    PlayerManager playerManager;
    Rigidbody rb;

    int IDamageDealer.damage
    {
        get { return damage; }
        set { }
    }
    public DamageType damageType { get => DamageType.Bullet; set => throw new System.NotImplementedException(); }

    public DamageSource damageSource
    {
        get => DamageSource.Friendly;
        
        set{}
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, bulletSpeed, 0));
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);
        playerPosition = player.transform.position;
        if (Vector3.Distance(playerPosition, transform.position) >= bulletDistance)
        {
            Destroy(this.gameObject);
        }

    }

    void OnTriggerEnter (Collider collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
        if (damageable != null && gameObject.tag != "Explosion")
        {
            print(damage + " damage taken");
            damageable.TakeDamage(this);
        }
        if(gameObject.tag == "Explosion")
        {
            GameObject expl = (GameObject)Resources.Load(Const.Grenade.GrenadeExplosion, typeof(GameObject));
            Instantiate(expl, gameObject.transform.position, Quaternion.identity);
            SoundManagerRework.Instance.PlayEffectOneShot(Resources.Load(Const.SFX.Explosion) as AudioClip);
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 1.5f);
            foreach (var hitCollider in hitColliders)
            {
                IDamageable damageItem = hitCollider.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
                if (damageItem != null)
                {
                    print(damage + " damage done");
                    damageItem.TakeDamage(this);
                }
            }
        }
        Destroy(this.gameObject);

    }

    int getDamage()
    {
        return damage;
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
    }
}
