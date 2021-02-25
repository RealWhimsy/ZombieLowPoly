using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWeaponInteraction : MonoBehaviour
{
    private GameObject player;
    private GameObject weaponPickup;
    private PlayerManager playerManager;
    private bool isOnWeapon;
    private GameObject weaponHand;
    
    // Animator hashes
    private static readonly int HasRpg = Animator.StringToHash("hasRPG");
    private static readonly int HasMelee = Animator.StringToHash("hasMelee");
    private static readonly int HasRifle = Animator.StringToHash("hasRifle");
    private static readonly int HasPistol = Animator.StringToHash("hasPistol");

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        weaponHand = GameObject.FindGameObjectWithTag("WeaponHand");
        RenderNewWeapon();

        SceneManager.sceneLoaded += ResetWeaponsOnGround;

        EventManager.StartListening(Const.Events.PlayerRespawned, SetRespawnState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isOnWeapon)
        {
            // TODO Trigger event for sound
            
            

            switch (weaponPickup.name)
            {
                case Const.WeaponNames.Deagle:
                    PickUpWeapon(Const.WeaponNames.Deagle);
                    break;

                case Const.WeaponNames.Ak47:
                    PickUpWeapon(Const.WeaponNames.Ak47);
                    break;
                
                case Const.WeaponNames.Aug:
                    PickUpWeapon(Const.WeaponNames.Aug);
                    break;

                case Const.WeaponNames.Python:
                    PickUpWeapon(Const.WeaponNames.Python);
                    break;

                case Const.WeaponNames.Mac10:
                    PickUpWeapon(Const.WeaponNames.Mac10);
                    break;

                case Const.WeaponNames.P90:
                    PickUpWeapon(Const.WeaponNames.P90);
                    break;
                
                case Const.WeaponNames.UMP45:
                    PickUpWeapon(Const.WeaponNames.UMP45);
                    break;
                
                case Const.WeaponNames.Barret:
                    PickUpWeapon(Const.WeaponNames.Barret);
                    break;
                
                case Const.WeaponNames.G36K:
                    PickUpWeapon(Const.WeaponNames.G36K);
                    break;
                
                case Const.WeaponNames.MP5:
                    PickUpWeapon(Const.WeaponNames.MP5);
                    break;

                case Const.WeaponNames.Scar:
                    PickUpWeapon(Const.WeaponNames.Scar);
                    break;

                case Const.WeaponNames.M4:
                    PickUpWeapon(Const.WeaponNames.M4);
                    break;

                case Const.WeaponNames.Awp:
                    PickUpWeapon(Const.WeaponNames.Awp);
                    break;

                case Const.WeaponNames.Svd:
                    PickUpWeapon(Const.WeaponNames.Svd);
                    break;

                case Const.WeaponNames.TwoBarrel:
                    PickUpWeapon(Const.WeaponNames.TwoBarrel);
                    break;

                case Const.WeaponNames.Spas:
                    PickUpWeapon(Const.WeaponNames.Spas);
                    break;

                case Const.WeaponNames.M249:
                    PickUpWeapon(Const.WeaponNames.M249);
                    break;

                case Const.WeaponNames.Rpg:
                    PickUpWeapon(Const.WeaponNames.Rpg);
                    break;

                case Const.WeaponNames.RamboKnife:
                    PickUpWeapon(Const.WeaponNames.RamboKnife);
                    break;

                case Const.WeaponNames.PoliceBat:
                    PickUpWeapon(Const.WeaponNames.PoliceBat);
                    break;
            }

            EventManager.TriggerEvent(Const.Events.WeaponSwapped);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchWeapon();
            RenderNewWeapon();
        }
    }

    /**
     * Called when switching weapons that the player already has in his inventory (TAB by default)
     * Switches to the next weapon.
     * Currently the player can only have two weapons, but this should be "future proof" if we add more inventory slots
     */
    private void SwitchWeapon()
    {
        playerManager.ActiveWeaponIndex++;
        SoundManagerRework.Instance.PlayEffectOneShot(Resources.Load(Const.SFX.WeaponPickup) as AudioClip);
        // Reset back to first weapon if Array is out of bounds, or not all slots are filled with weapons
        if (playerManager.ActiveWeaponIndex > Const.MaxWeaponIndex ||
            playerManager.ActiveWeaponIndex >= playerManager.CurrentlyEquippedWeapons)
        {
            playerManager.ActiveWeaponIndex = 0;
        }

        EventManager.TriggerEvent(Const.Events.WeaponSwapped);
    }

    /**
     * Picks up a weapon from the ground, if the player is standing on top of one.
     * Drops the players currently equipped weapon if "inventory" is full.
     */
    private void PickUpWeapon(string weaponName)
    {
        SoundManagerRework.Instance.PlayEffectOneShot(Resources.Load(Const.SFX.WeaponPickup) as AudioClip);

        // if the player "picks up" a weapon they already have, the ammo gets refilled
        foreach (var weapon in playerManager.WeaponArray)
        {
            if (weapon == null)
            {
                continue;
            }
            
            if (weapon.Name.Equals(weaponName))
            {
                weapon.Magazines++;
                weapon.ShotsInCurrentMag = weapon.MaxMagazineSize;
                
                // Destroy weapon on the ground
                Destroy(weaponPickup);
                return;
            }
        }


        for (int i = 0; i <= Const.MaxWeaponIndex; i++)
        {
            if (playerManager.WeaponArray[i] is null)
            {
                playerManager.WeaponArray[i] = WeaponStats.weaponStatDict[weaponName];
                playerManager.ActiveWeaponIndex = i;
                playerManager.CurrentlyEquippedWeapons++;
                RenderNewWeapon();
                Destroy(weaponPickup);
                EventManager.TriggerEvent(Const.Events.WeaponPickedUp);
                return;
            }
        }

        // Drop currently equipped gun when both slots are already used
        var currentWeapon = playerManager.GetActiveWeapon();
        DropCurrentWeapon(currentWeapon);

        playerManager.WeaponArray[playerManager.ActiveWeaponIndex] = WeaponStats.weaponStatDict[weaponName];

        // Destroy weapon on the ground
        Destroy(weaponPickup);
        
        // Broadcast Event and show new weapon
        EventManager.TriggerEvent(Const.Events.WeaponPickedUp);
        RenderNewWeapon();
    }

    private void RenderNewWeapon()
    {
        foreach (Transform weaponTransform in weaponHand.transform)
        {
            weaponTransform.gameObject.SetActive(
                weaponTransform.gameObject.name.Equals(playerManager.GetActiveWeapon().Name));
        }
        SetAnimation();
    }

    private void DropCurrentWeapon(Weapon weapon)
    {
        GameObject weaponPrefab = Resources.Load("Prefabs/" + weapon.Name) as GameObject;
        
        GameObject droppedGun = Instantiate(weaponPrefab, player.transform.position + new Vector3(0f, 0.4f, 0f),
            Quaternion.identity * Quaternion.Euler(0f, 0f, -90f));

        if (droppedGun.transform.name.Contains("(Clone)"))
        {
            droppedGun.transform.name = droppedGun.transform.name.Replace("(Clone)", "").Trim();
        }
    }

    private void SetRespawnState()
    {
        /*SwitchWeapon();
        RenderNewWeapon();*/
    }

    private void ResetWeaponsOnGround(Scene scene, LoadSceneMode mode)
    {
        foreach (var weapon in WeaponStats.weaponStatDict.Values)
        {
            weapon.ShotsInCurrentMag = weapon.MaxMagazineSize;
            weapon.Magazines = weapon.MaxMagazines;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("weapon"))
        {
            weaponPickup = collider.gameObject;
            isOnWeapon = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("weapon"))
        {
            isOnWeapon = false;
        }
    }

    private void SetAnimation(){
        playerManager.anim.SetBool(HasRpg, false);
        playerManager.anim.SetBool(HasMelee, false);
        playerManager.anim.SetBool(HasRifle, false);
        playerManager.anim.SetBool(HasPistol, false);

        switch (playerManager.GetActiveWeapon().WeaponType)
        {
            case WeaponType.Pistol:
                playerManager.anim.SetBool(HasPistol, true);
                break;
            case WeaponType.Rifle:
            case WeaponType.Sniper:
            case WeaponType.Lmg:
            case WeaponType.Smg:
            case WeaponType.Shotgun:
                playerManager.anim.SetBool(HasRifle, true);
                break;
            case WeaponType.Rpg:
                playerManager.anim.SetBool(HasRpg, true);
                break;
            case WeaponType.Melee:
                playerManager.anim.SetBool(HasMelee, true);
                break;
        }
    }
}