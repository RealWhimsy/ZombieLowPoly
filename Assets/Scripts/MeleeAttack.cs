using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IDamageDealer
{
    private GameObject player;
    private PlayerManager playerManager;
    
    // List that contains all colliders already hit by melee attack. Deals no damage if already hit,
    // to prevent double hits on the same target
    private List<Collider> alreadyHitUnits;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        // Reset the list of already hit targets on new melee attack
        alreadyHitUnits = new List<Collider>();
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision);
        IDamageable damageable = collision.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;

        if (damageable != null && !alreadyHitUnits.Contains(collision))
        {
            Debug.Log("DAMAGE" + collision);
            alreadyHitUnits.Add(collision);
            damageable.TakeDamage(this);
        }
        
    }
   
    int IDamageDealer.damage
    {
        get => playerManager.GetActiveWeapon().Damage;
        set { }
    }
    public DamageType damageType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public DamageSource damageSource
    {
        get => DamageSource.Friendly;
        set { }
    }
}
