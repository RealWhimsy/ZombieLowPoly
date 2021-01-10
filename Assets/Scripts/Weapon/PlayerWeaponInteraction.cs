using System;
using System.Linq;
using UnityEngine;

public class PlayerWeaponInteraction : MonoBehaviour
{
    private GameObject player;
    private GameObject weaponPickup;
    private PlayerManager playerManager;
    private bool isOnWeapon;
    private GameObject weaponHand;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        weaponHand = GameObject.FindGameObjectWithTag("WeaponHand");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isOnWeapon)
        {
            SoundManager.PlaySound(SoundManager.Sound.WeaponSwitch);
            
            

            switch (weaponPickup.name)
            {
                case Const.WeaponNames.Deagle:
                    PickUpWeapon(Const.WeaponNames.Deagle);
                    setAnimation();
                    break;

                case Const.WeaponNames.Ak47:
                    PickUpWeapon(Const.WeaponNames.Ak47);
                    setAnimation();
                    break;
                
                case Const.WeaponNames.Aug:
                    PickUpWeapon(Const.WeaponNames.Aug);
                    setAnimation();
                    break;

                case Const.WeaponNames.Python:
                    PickUpWeapon(Const.WeaponNames.Python);
                    setAnimation();
                    break;

                case Const.WeaponNames.Mac10:
                    PickUpWeapon(Const.WeaponNames.Mac10);
                    setAnimation();
                    break;

                case Const.WeaponNames.P90:
                    PickUpWeapon(Const.WeaponNames.P90);
                    setAnimation();
                    break;

                case Const.WeaponNames.Scar:
                    PickUpWeapon(Const.WeaponNames.Scar);
                    setAnimation();
                    break;

                case Const.WeaponNames.M4:
                    PickUpWeapon(Const.WeaponNames.M4);
                    setAnimation();
                    break;

                case Const.WeaponNames.Awp:
                    PickUpWeapon(Const.WeaponNames.Awp);
                    setAnimation();
                    break;

                case Const.WeaponNames.Svd:
                    PickUpWeapon(Const.WeaponNames.Svd);
                    setAnimation();
                    break;

                case Const.WeaponNames.TwoBarrel:
                    PickUpWeapon(Const.WeaponNames.TwoBarrel);
                    setAnimation();
                    break;

                case Const.WeaponNames.Spas:
                    PickUpWeapon(Const.WeaponNames.Spas);
                    setAnimation();
                    break;

                case Const.WeaponNames.M249:
                    PickUpWeapon(Const.WeaponNames.M249);
                    setAnimation();
                    break;

                case Const.WeaponNames.Rpg:
                    PickUpWeapon(Const.WeaponNames.Rpg);
                    setAnimation();
                    break;

                case Const.WeaponNames.RamboKnife:
                    PickUpWeapon(Const.WeaponNames.RamboKnife);
                    setAnimation();
                    break;

                case Const.WeaponNames.PoliceBat:
                    PickUpWeapon(Const.WeaponNames.PoliceBat);
                    setAnimation();
                    break;
            }

            EventManager.TriggerEvent(Const.Events.WeaponSwapped);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchWeapon();
            RenderNewWeapon();
            setAnimation();
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
    }

    private void DropCurrentWeapon(Weapon weapon)
    {
        GameObject weaponPrefab = Resources.Load("Prefabs/" + weapon.Name) as GameObject;
        
        GameObject droppedGun = Instantiate(weaponPrefab, player.transform.position + new Vector3(0f, 0.4f, 0f),
            Quaternion.identity * Quaternion.Euler(0f, 0f, -90f));

        droppedGun.GetComponent<BulletContainer>()
            .SetValues(true, weapon.ShotsInCurrentMag, weapon.Magazines);
        if (droppedGun.transform.name.Contains("(Clone)"))
        {
            droppedGun.transform.name = droppedGun.transform.name.Replace("(Clone)", "").Trim();
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

    private void setAnimation(){
        String weaponType = getWeaponType();
        playerManager.anim.SetBool("hasRPG", false);
        playerManager.anim.SetBool("hasMelee", false);
        playerManager.anim.SetBool("hasRifle", false);
        playerManager.anim.SetBool("hasPistol", false);

        if (weaponType == "Pistol")
        {
            playerManager.anim.SetBool("hasPistol", true);
        }
        if (weaponType == "Rifle")
        {
            playerManager.anim.SetBool("hasRifle", true);
        }
        if (weaponType == "Rpg")
        {
            playerManager.anim.SetBool("hasRPG", true);
        }
        if (weaponType == "Melee")
        {
            playerManager.anim.SetBool("hasMelee", true);
        }

    }
    public string getWeaponType()
    {
        var currentWeapon = playerManager.GetActiveWeapon();
        Debug.Log(currentWeapon.Name);

        if(currentWeapon.Name == "w_DesertEagle" || currentWeapon.Name == "w_python")
        {
            return "Pistol";
        }
        if (currentWeapon.Name == "w_rambo_knife" || currentWeapon.Name == "w_policebatton")
        {
            return "Melee";
        }
        if (currentWeapon.Name == "w_rpg")
        {
            return "Rpg";
        }

        return "Rifle";

    }
}