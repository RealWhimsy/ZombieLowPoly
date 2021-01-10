using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageable
{

    public int maxHealth = 200;
    public int armor = 5;
    public HealthBar healthBar;

    int currentHealth;
    bool dead = false;
    
    private Weapon[] weaponArray = new Weapon[Const.MaxNumWeapons];
    private Weapon activeWeapon;
    private int activeWeaponIndex;
    private int currentlyEquippedWeapons;
    private GameObject blood;

    private static readonly int Melee = Animator.StringToHash("melee");

    Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        blood = Resources.Load("Prefabs/Blood") as GameObject;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        anim = GetComponent<Animator>();
        PrepareWeaponArray();
        EventManager.StartListening(Const.Events.MeleeAttack, HandleMeleeAttack);
    }

    private void PrepareWeaponArray()
    {
        weaponArray[Const.FirstWeaponIndex] = new Weapon(WeaponStats.weaponStatDict[Const.WeaponNames.Deagle]);
        weaponArray[Const.FirstWeaponIndex].Name = Const.WeaponNames.Deagle;
        activeWeaponIndex = Const.FirstWeaponIndex;
        currentlyEquippedWeapons = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            dead = true;
            anim.SetBool("isDead", true);
        }
    }

    public void TakeDamage(IDamageDealer damageDealer)
    {
        int finalDamage = damageDealer.damage - armor;
        if (finalDamage < 0)
        {
            finalDamage = 0;
        }
        Instantiate(blood, transform.position, transform.rotation);
        currentHealth -= finalDamage;
        healthBar.SetHealth(currentHealth);

        Debug.Log("Player took " + finalDamage + " damage. Current Health: " + currentHealth);
    }

    void HandleMeleeAttack()
    {
        anim.SetTrigger(Melee);
    }

    public bool isDead()
    {
        return dead;
    }

    public Weapon GetActiveWeapon()
    {
        return weaponArray[activeWeaponIndex];
    }

    public int ActiveWeaponIndex
    {
        get => activeWeaponIndex;
        set => activeWeaponIndex = value;
    }

    public Weapon[] WeaponArray
    {
        get => weaponArray;
        set => weaponArray = value;
    }

    public int CurrentlyEquippedWeapons
    {
        get => currentlyEquippedWeapons;
        set => currentlyEquippedWeapons = value;
    }
}
