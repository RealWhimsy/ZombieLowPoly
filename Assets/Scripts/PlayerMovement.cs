using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 0;
    public Animator anim;

    private Camera mainCamera;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private PlayerManager playerManager;
    private GameObject player;
    private Gun weaponLogic;
    private GameObject weaponHand;
    private bool firstWeapon;
    private Weapon firstGun;
    private Weapon secondGun;
    private GameObject currentWeapon;
    private GameObject weaponPickup;
    private bool onWeapon;

    private GameObject bullet;
    public GameObject bulletSpawner;
    public float shotCooldown;
    private float shotTime;
    public int magazineSize;
        public int magazines;


    private void Start() {
        addGunToPlayer();

        controller = GetComponent<CharacterController>();
        shotTime = Time.time;
        weaponHand = GameObject.FindGameObjectWithTag("WeaponHand");
        currentWeapon = getActiveWeapon();
        firstWeapon = true;
        setWeaponStats();
        mainCamera = FindObjectOfType<Camera>();
        playerManager = GetComponent<PlayerManager>();
        firstGun = new Weapon(currentWeapon, magazines, magazineSize, shotCooldown, bullet);
        weaponLogic.bulletsRemaining = firstGun.magazineSize;
        weaponLogic.magazinesRemaining = firstGun.magazines;
        weaponLogic.initBulletUi();
    }

    private void addGunToPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weaponLogic = player.AddComponent<Gun>();
    }

    private void Update() {
        if (!playerManager.isDead())
        {
            BaseMovement();
            MouseMovement();
            AmmoTracker();
            PickupWeapon();
        }
    }

    private void BaseMovement() {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        
        moveDirection = new Vector3(moveX,0,moveZ);
        moveDirection *= moveSpeed;
        controller.Move(moveDirection);

        anim.SetFloat("horizontal", moveX);
        anim.SetFloat("vertical", moveZ);
    }

    private void PickupWeapon()
    {
        if (onWeapon && Input.GetKeyDown("e"))
        { 
            if (weaponPickup != null && weaponPickup.tag == "weapon")
            {
                if (!ReferenceEquals(secondGun, null))
                {

                }
                else
                {
                    currentWeapon = getWeapon(weaponPickup);
                    setWeaponStats();
                    secondGun = new Weapon(currentWeapon, magazines, magazineSize, shotCooldown, bullet);
                    firstGun.weapon.SetActive(false);
                    secondGun.weapon.SetActive(true);
                    weaponLogic.bulletsRemaining = secondGun.magazineSize;
                    weaponLogic.magazinesRemaining = secondGun.magazines;
                    weaponLogic.UpdateAmmoUi();
                    Destroy(weaponPickup);
                    firstWeapon = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        weaponPickup = collider.gameObject;
        onWeapon = true;
       
    }
    private void OnTriggerExit()
    {
       onWeapon = false;

    }

    private void MouseMovement() {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if(groundPlane.Raycast(cameraRay, out rayLength)) {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
        if (Input.GetMouseButton(0) && !weaponLogic.reloading)
        {
            if (weaponLogic.bulletsRemaining > 0 && (Time.time - shotTime > getEquipedGun().shotCooldown))
            {
                weaponLogic.bulletsRemaining -= 1;
                weaponLogic.ReduceBulletUi();
                reduceActiveWeaponMunition("magazineSize");
                Shoot();
                shotTime = Time.time;
            }
        }
        if (Input.GetKeyDown("r") && getEquipedGun().magazineSize != getEquipedGun().maxMagazineSize)
        {
            Weapon current = getEquipedGun();
            if (current.magazines > 0)
            {
                reduceActiveWeaponMunition("magazines");
                getEquipedGun().magazineSize = getEquipedGun().maxMagazineSize;
                StartCoroutine(weaponLogic.Reload(current.maxMagazineSize));
            }
        }
        if (Input.GetKeyDown("t"))
        {
            switchWeapon();
        }
    }
    private void switchWeapon()
    {
        if (!ReferenceEquals(secondGun, null) && !weaponLogic.reloading)
        {
            if (firstWeapon)
            {
                firstGun.weapon.SetActive(false);
                secondGun.weapon.SetActive(true);
                weaponLogic.bulletsRemaining = secondGun.magazineSize;
                weaponLogic.magazinesRemaining = secondGun.magazines;
                weaponLogic.UpdateAmmoUi();
                firstWeapon = false;
            }
            else
            {
                secondGun.weapon.SetActive(false);
                firstGun.weapon.SetActive(true);
                weaponLogic.bulletsRemaining = firstGun.magazineSize;
                weaponLogic.magazinesRemaining = firstGun.magazines;
                weaponLogic.UpdateAmmoUi();
                firstWeapon = true;
            }
        }
    }

    private void reduceActiveWeaponMunition(String magOrMagSize)
    {

        Weapon current = getEquipedGun();
        if (magOrMagSize == "magazines")
        {
            current.magazines -= 1;
        }
        if (magOrMagSize == "magazineSize")
        {
            current.magazineSize -= 1;
        }
    }

    private Weapon getEquipedGun()
    {
        if (firstWeapon)
        {
            return firstGun;
        }
        else
        {
            return secondGun;
        }
    }

    void Shoot()
    {
        Transform playerBullet = Instantiate(getEquipedGun().bullet.transform, bulletSpawner.transform.position, bulletSpawner.transform.rotation);
        Bullet bulletScript = playerBullet.GetComponent<Bullet>();
        bulletScript.setDamage(playerManager.damage);
    }

    private void AmmoTracker()
    {
        if (weaponLogic.bulletsRemaining <= 0 && weaponLogic.magazinesRemaining <= 0)
        {
            weaponLogic.OutOfAmmoText();
        }

        if (weaponLogic.bulletsRemaining <= 0 && weaponLogic.magazinesRemaining > 0)
        {
            weaponLogic.ReloadText();
        }
    }

    private void setWeaponStats()
    {
        if(currentWeapon.name == "w_DesertEagle")
        {
            magazineSize = 12;
            magazines = 3;
            shotCooldown = (float)0.7;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
        }
        if (currentWeapon.name == "w_ak47")
        {
            magazineSize = 15;
            magazines = 3;
            shotCooldown = (float)0.3;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
        }
        if (currentWeapon.name == "w_ak47_double_drum")
        {
            magazineSize = 30;
            magazines = 2;
            shotCooldown = (float)0.3;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
        }
        if (currentWeapon.name == "w_python")
        {
            magazineSize = 9;
            magazines = 4;
            shotCooldown = (float)0.6;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
        }
        if (currentWeapon.name == "w_rpg")
        {
            magazineSize = 5;
            magazines = 0;
            shotCooldown = (float)2;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/projectile_rpg") as GameObject;
        }
        if (currentWeapon.name == "w_spas")
        {
            magazineSize = 8;
            magazines = 3;
            shotCooldown = (float)1;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/shotgun_shell") as GameObject;
        }
        if (currentWeapon.name == "w_svd")
        {
            magazineSize = 5;
            magazines = 3;
            shotCooldown = (float)1.5;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
        }
    }
    private GameObject getActiveWeapon()
    {
        for (int i = 0; i < weaponHand.transform.childCount; i++)
        {
            if (weaponHand.transform.GetChild(i).gameObject.activeSelf == true)
            {
                return weaponHand.transform.GetChild(i).gameObject;
            }
        }
        return null;
    }
    
    private GameObject getWeapon(GameObject weapon)
        {
            for (int i = 0; i < weaponHand.transform.childCount; i++)
            {
                if (weaponHand.transform.GetChild(i).gameObject.name == weapon.name)
                {
                    return weaponHand.transform.GetChild(i).gameObject;
                }
            }
        return null;
        }
}
