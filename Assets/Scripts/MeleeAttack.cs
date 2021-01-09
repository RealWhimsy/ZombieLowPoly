using UnityEngine;

public class MeleeAttack : MonoBehaviour, IDamageDealer
{
    private GameObject player;
    private PlayerManager playerManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision);
        IDamageable damageable = collision.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
        if (damageable != null)
        {
            Debug.Log("DAMAGE" + collision);
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
