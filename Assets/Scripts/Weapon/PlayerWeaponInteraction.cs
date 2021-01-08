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
                    break;

                case Const.WeaponNames.Ak47:
                    PickUpWeapon(Const.WeaponNames.Ak47);
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
        
        // Broadcast Event
        EventManager.TriggerEvent(Const.Events.WeaponPickedUp);
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

        // TODO set correct path to prefab for Instantiate
        // GameObject droppedGun = Instantiate(weaponPrefab, player.transform.position + new Vector3(0f, 0.4f, 0f),
        //     Quaternion.identity * Quaternion.Euler(0f, 0f, -90f));

        // droppedGun.GetComponent<BulletContainer>()
        //     .SetValues(true, weapon.ShotsInCurrentMag, weapon.Magazines);
        // if (droppedGun.transform.name.Contains("(Clone)"))
        // {
        //     droppedGun.transform.name = droppedGun.transform.name.Replace("(Clone)", "").Trim();
        // }
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
}