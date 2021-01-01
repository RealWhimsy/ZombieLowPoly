using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IDamageDealer
{
    int damage;

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision);
        IDamageable damageable = collision.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
        if (damageable != null)
        {
            Debug.Log("DAMAGEE" + collision);
            damageable.TakeDamage(this);
        }
        
    }
    public void setDamage(int damage)
    {
        this.damage = damage;
    }
    int IDamageDealer.damage
    {
        get { return damage; }
        set { }
    }
    public DamageType damageType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    int getDamage()
    {
        return damage;
    }
}
