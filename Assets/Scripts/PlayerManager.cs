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

    public Animator anim;
    private static readonly int Melee = Animator.StringToHash("melee");
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
        EventManager.StartListening(Const.Events.LevelLoaded, SetSpawnStats);
        EventManager.StartListening(Const.Events.LevelLoaded, ResetSupplies);
        SetSpawnStats();
        PrepareWeaponArray();
        FindHealthBar();

    }

    private void SetSpawnStats()
    {
		grenades = 2;
		armor = 5;
        triggerPlayerDeadEvent = false;
        dead = false;
        anim.SetBool(IsDead, false);
        maxHealth = 200;
        currentHealth = maxHealth;
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

        if (GetActiveWeapon().Magazines >= Const.Magazines.MaxMagazines)
        {
            GetActiveWeapon().Magazines = Const.Magazines.MaxMagazines;
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

    private void ResetSupplies()
    {
        foreach (var weapon in weaponArray)
        {
            weapon.Magazines = weapon.MaxMagazines;
            weapon.ShotsInCurrentMag = weapon.MaxMagazineSize;
        }
        EventManager.TriggerEvent(Const.Events.UpdateAmmoUi);
    }

    private void FindHealthBar()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
    }

    private void CheckIfPlayerIsUnderWater()
    {
        if (!SceneManager.GetActiveScene().name.Equals(Const.SceneNames.PirateBay) ||
            SceneManager.GetActiveScene().name.Equals(Const.SceneNames.Desert)) return;
        
        if (SceneManager.GetActiveScene().name.Equals(Const.SceneNames.Desert))
        {
            if (gameObject.transform.position.y <= 22)
            {
                currentHealth = -10;
            }
        }

        if (SceneManager.GetActiveScene().name.Equals(Const.SceneNames.PirateBay))
        {
            if (gameObject.transform.position.y <= 36)
            {
                currentHealth = -10;
            }
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        CheckIfPlayerIsUnderWater();
        if (currentHealth <= 0 && !dead)
        {
            dead = true;
            anim.SetBool(IsDead, true);
            if (!triggerPlayerDeadEvent)
            {
                triggerPlayerDeadEvent = true;
                SoundManagerRework.Instance.PlayEffectOneShot(Resources.Load(Const.SFX.Death) as AudioClip);
                EventManager.TriggerEvent(Const.Events.PlayerDead);
                StartCoroutine(Respawn());
            }

        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(Const.Player.RespawnTime);
        SetSpawnStats();
        healthBar.SetHealth(currentHealth);
        ResetSupplies();
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
        DDAManager.DamageTaken += finalDamage;
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
