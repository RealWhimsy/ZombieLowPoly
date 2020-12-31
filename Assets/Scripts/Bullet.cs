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
    public DamageType damageType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        damage = playerManager.damage;
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
        if (damageable != null)
        {
            print(damage + " damage taken");
            damageable.TakeDamage(this);
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
