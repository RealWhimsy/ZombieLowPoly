using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour, IDamageable
{

    public int maxHealth;
    public int armor;
    public int grenades;
    int currentHealth;
    bool dead;
    public HealthBar healthBar;
    private bool triggerPlayerDeadEvent = false;
    
    private Weapon[] weaponArray = new Weapon[Const.MaxNumWeapons];
    private Weapon activeWeapon;
    private int activeWeaponIndex;
    private int currentlyEquippedWeapons;
    private GameObject blood;

    private static readonly int Melee = Animator.StringToHash("melee");

    public Animator anim;
    private static readonly int Shoot = Animator.StringToHash("shoot");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private static readonly int HasPistol = Animator.StringToHash("hasPistol");

    // Start is called before the first frame update
    void Awake()
    {
        blood = Resources.Load("Prefabs/Blood") as GameObject;
        anim = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Animator>();
        EventManager.StartListening(Const.Events.MeleeAttack, HandleMeleeAttack);
		EventManager.StartListening(Const.Events.InteractibleCollected, AddSupplies);
        setSpawnStats();
        FindHealthBar();

    }

    private void setSpawnStats()
    {
		grenades = 2;
		armor = 5;
        triggerPlayerDeadEvent = false;
        dead = false;
        anim.SetBool(IsDead, false);
        maxHealth = 200;
        currentHealth = maxHealth;
        armor = 5;
        PrepareWeaponArray();
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
        anim.SetBool(HasPistol, true);
        weaponArray[Const.FirstWeaponIndex].Name = Const.WeaponNames.Deagle;
        activeWeaponIndex = Const.FirstWeaponIndex;
        currentlyEquippedWeapons = 1;
    }

    // Adds magazines, grenades and health to player if interactible box was collected
    private void AddSupplies()
    {
        int pickUpMagazines = 1;
        int pickUpGrenades = 1;
        int pickUpHealth = maxHealth / 10;
        if(GameAssets.i.GenerateRandomNumber(0,1) == 1)
        {
            pickUpMagazines = 2;
            pickUpHealth = maxHealth / 5;
        }

        grenades += pickUpGrenades;
        if (grenades >= Const.Grenade.MaxGrenades)
        {
            grenades = Const.Grenade.MaxGrenades;
        }
        currentHealth += pickUpHealth;
        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
        GetActiveWeapon().Magazines += pickUpMagazines;
        EventManager.TriggerEvent(Const.Events.UpdateAmmoUi);
    }

    private void FindHealthBar()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0 && !dead)
        {
            dead = true;
            anim.SetBool(IsDead, true);
            if (!triggerPlayerDeadEvent)
            {
                triggerPlayerDeadEvent = true;
                EventManager.TriggerEvent(Const.Events.PlayerDead);
                StartCoroutine(Respawn());
            }

        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(Const.Player.RespawnTime);
		if(currentlyEquippedWeapons > 1) {
			weaponArray[Const.SecondWeaponIndex] = null;
		}
        setSpawnStats();
		EventManager.TriggerEvent(Const.Events.PlayerRespawned);
    }

    public void TakeDamage(IDamageDealer damageDealer)
    {
        // Do not take damage if source is from the player (prevent friendly fire)
        if (damageDealer.damageSource == DamageSource.Friendly)
        {
            return;
        }
        
        int finalDamage = damageDealer.damage - armor;
        if (finalDamage < 0)
        {
            finalDamage = 0;
        }
        Instantiate(blood, transform.position, transform.rotation);
        currentHealth -= finalDamage;
        healthBar.SetHealth(currentHealth);
    }

    void HandleMeleeAttack()
    {
        anim.SetTrigger(Shoot);
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
