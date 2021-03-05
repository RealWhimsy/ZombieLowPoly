using System;
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
    private bool ddaCountedAsHit;
    private bool hitDamageableTarget;


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
        if (damageable != null && !gameObject.CompareTag("Explosion"))
        {
            damageable.TakeDamage(this);
            hitDamageableTarget = true;
        }
        if(gameObject.CompareTag("Explosion") && collision.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }

        if (collision.gameObject.CompareTag("DDAZombieHitbox") && !ddaCountedAsHit)
        {
            // one bullet can only count as one hit for the DDA calculation
            ddaCountedAsHit = true;
            EventManager.TriggerEvent(Const.Events.ShotHitDDAZone);
        }

        // do not destroy bullets if they hit non-damagable objects such as interactibles
        if (hitDamageableTarget)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        CheckForEnvironmentCollision(other);
    }

    /**
     * Check if the bullet collides with any impassable environment terrain
     * If yes, destroys the bullet
     */
    private void CheckForEnvironmentCollision(Collision other)
    {
        bool bulletShouldBeDestroyed = !(other.gameObject.CompareTag("InteractiblePickup") || other.gameObject.CompareTag("weapon"));
        
        // check all parents of the other GameObject for the EnvironmentBlocker tag
        Transform otherTransform = other.transform;

        while (otherTransform.parent != null)
        {
            if (other.gameObject.CompareTag("InteractiblePickup") || other.gameObject.CompareTag("weapon"))
            {
                bulletShouldBeDestroyed = false;
                break;
            }

            otherTransform = otherTransform.parent.transform;
        }

        if (bulletShouldBeDestroyed)
        {
            HandleEnvironmentCollision();
        }
    }

    private void HandleEnvironmentCollision()
    {
        if (gameObject.CompareTag("Explosion"))
        {
            Explode();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Explode()
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
        Destroy(gameObject);
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
