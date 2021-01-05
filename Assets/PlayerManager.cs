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
    
    private Weapon[] weaponArray = new Weapon[Const.MAX_NUM_WEAPONS];
    private Weapon activeWeapon;
    private int activeWeaponIndex;

    Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        anim = GetComponent<Animator>();
        PrepareWeaponArray();
        
    }

    private void PrepareWeaponArray()
    {
        weaponArray[Const.FIRST_WEAPON_INDEX] = new Weapon(WeaponStats.weaponStatDict[Const.WeaponNames.DEAGLE]);
        activeWeaponIndex = Const.FIRST_WEAPON_INDEX;
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

        currentHealth -= finalDamage;
        healthBar.SetHealth(currentHealth);

        Debug.Log("Player took " + finalDamage + " damage. Current Health: " + currentHealth);
    }

    public bool isDead()
    {
        return dead;
    }

    public Weapon GetActiveWeapon()
    {
        return weaponArray[activeWeaponIndex];
        
        // Should never reach the code below
        print("Weapon Index " + activeWeaponIndex + " has no assigned weapon");
        return null;
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
}
