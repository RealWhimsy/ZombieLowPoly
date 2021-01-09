using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour, IDamageable
{

    private int maxHealth;
    private int armor;
    public HealthBar healthBar;

    int currentHealth;
    bool dead = false;
    
    private Weapon[] weaponArray = new Weapon[Const.MaxNumWeapons];
    private Weapon activeWeapon;
    private int activeWeaponIndex;
    private int currentlyEquippedWeapons;
    
    private static readonly int Melee = Animator.StringToHash("melee");

    Animator anim;
    
    void Awake()
    {
        maxHealth = 200;
        currentHealth = maxHealth;
        armor = 5;
        anim = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Animator>();
        PrepareWeaponArray();
        EventManager.StartListening(Const.Events.MeleeAttack, HandleMeleeAttack);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindHealthBar();
    }

    private void PrepareWeaponArray()
    {
        weaponArray[Const.FirstWeaponIndex] = new Weapon(WeaponStats.weaponStatDict[Const.WeaponNames.Deagle]);
        weaponArray[Const.FirstWeaponIndex].Name = Const.WeaponNames.Deagle;
        activeWeaponIndex = Const.FirstWeaponIndex;
        currentlyEquippedWeapons = 1;
    }

    private void FindHealthBar()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
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
