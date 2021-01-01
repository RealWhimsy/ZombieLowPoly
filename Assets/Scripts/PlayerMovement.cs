using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 0;
    public Animator anim;

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

    private Camera mainCamera;


    private GameObject bullet;
    public GameObject bulletSpawner;
    public float shotCooldown;
    private float shotTime;
    public int magazineSize;
    public int magazines;
    private float bulletSpread;
    private int damage;
    private int meleeDamage;
    private GameObject meleeArea;
    private bool melee;


    private void Start() {

        addGunToPlayer();
        controller = GetComponent<CharacterController>();
        shotTime = Time.time;
        meleeDamage = 20;
        melee = false;
        meleeArea = (GameObject) GameObject.Find("MeleeArea");
        meleeArea.SetActive(false);
        weaponHand = GameObject.FindGameObjectWithTag("WeaponHand");
        currentWeapon = getActiveWeapon();
        firstWeapon = true;
        setWeaponStats();
        mainCamera = FindObjectOfType<Camera>();
        playerManager = GetComponent<PlayerManager>();

        firstGun = new Weapon(currentWeapon, magazines, magazineSize, shotCooldown, bullet, damage, melee);
        weaponLogic.bulletsRemaining = firstGun.magazineSize;
        weaponLogic.magazinesRemaining = firstGun.magazines;
        weaponLogic.initBulletUi();
    }

    private void addGunToPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weaponLogic = player.AddComponent<Gun>();
    }

    private void Update()
    {
        if (!playerManager.isDead())
        {
            BaseMovement();
            MouseMovement();
            AmmoTracker();
            PickupWeapon();
        }
    }

    private void BaseMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ);
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
                    currentWeapon = getWeapon(weaponPickup);
                    
                    Destroy(weaponPickup);
                    GameObject weapPrefab = Resources.Load("Prefabs/" + getEquipedGun().weapon.name) as GameObject;
                    
                    GameObject droppedGun = Instantiate(weapPrefab, player.transform.position + new Vector3(0f ,0.4f ,0f ), Quaternion.identity * Quaternion.Euler(0f, 0f, -90f));
                    
                    droppedGun.GetComponent<BulletContainer>().SetValues(true, getEquipedGun().magazines, getEquipedGun().magazineSize);
                    if (droppedGun.transform.name.Contains("(Clone)"))
                    {
                        droppedGun.transform.name = droppedGun.transform.name.Replace("(Clone)", "").Trim();
                    }

                    if (weaponPickup.GetComponent<BulletContainer>() == null || !weaponPickup.GetComponent<BulletContainer>().GetUsedGun())
                    {
                        setWeaponStats();
                        if (firstWeapon) { 
                            firstGun.weapon.SetActive(false); 
                            firstGun = new Weapon(currentWeapon, magazines, magazineSize, shotCooldown, bullet, damage, melee); 
                            firstGun.weapon.SetActive(true);
                            weaponLogic.bulletsRemaining = firstGun.magazineSize;
                            weaponLogic.magazinesRemaining = firstGun.magazines;
                        } 
                        else {
                            secondGun.weapon.SetActive(false); 
                            secondGun = new Weapon(currentWeapon, magazines, magazineSize, shotCooldown, bullet, damage, melee); 
                            secondGun.weapon.SetActive(true);
                            weaponLogic.bulletsRemaining = secondGun.magazineSize;
                            weaponLogic.magazinesRemaining = secondGun.magazines;
                        }
                        if (!melee)
                        {
                            weaponLogic.UpdateAmmoUi();
                        }
                        else
                        {
                            weaponLogic.removeUi();
                        }
                        
                    }
                    if (weaponPickup.GetComponent<BulletContainer>().GetUsedGun())
                    {
                            Debug.Log(currentWeapon);
                            setWeaponStats();
                            magazineSize = weaponPickup.GetComponent<BulletContainer>().GetMagazineSize();
                            magazines = weaponPickup.GetComponent<BulletContainer>().GetMagazines();
                        if (firstWeapon)
                            {
                                firstGun.weapon.SetActive(false);
                                firstGun = new Weapon(currentWeapon, magazines, magazineSize, shotCooldown, bullet, damage, melee);
                                firstGun.weapon.SetActive(true);
                                weaponLogic.bulletsRemaining = firstGun.magazineSize;
                                weaponLogic.magazinesRemaining = firstGun.magazines;
                            if (!melee)
                            {
                                weaponLogic.UpdateAmmoUi();
                            }
                            else
                            {
                                weaponLogic.removeUi();
                            }
                        }
                            else
                            {
                                secondGun.weapon.SetActive(false);
                                secondGun = new Weapon(currentWeapon, magazines, magazineSize, shotCooldown, bullet, damage, melee);
                                secondGun.weapon.SetActive(true);
                                weaponLogic.bulletsRemaining = secondGun.magazineSize;
                                weaponLogic.magazinesRemaining = secondGun.magazines;
                            if (!melee)
                            {
                                weaponLogic.UpdateAmmoUi();
                            }
                            else
                            {
                                weaponLogic.removeUi();
                            }
                        }
                            
                            
                        }
                    
                    
                }
                else
                {

                    currentWeapon = getWeapon(weaponPickup);
                    setWeaponStats();
                    secondGun = new Weapon(currentWeapon, magazines, magazineSize, shotCooldown, bullet, damage, melee);
                    firstGun.weapon.SetActive(false);
                    secondGun.weapon.SetActive(true);
                    weaponLogic.bulletsRemaining = secondGun.magazineSize;
                    weaponLogic.magazinesRemaining = secondGun.magazines;
                    if (!melee)
                    {
                        weaponLogic.UpdateAmmoUi();
                    }
                    else
                    {
                        weaponLogic.removeUi();
                    }
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

    private void MouseMovement()
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
        if (Input.GetMouseButton(0) && !weaponLogic.reloading)
        {
            if (weaponLogic.bulletsRemaining > 0 && (Time.time - shotTime > getEquipedGun().shotCooldown) || melee)
            {
                if (melee)
                {
                    Shoot();
                    shotTime = Time.time;
                }
                else
                {
                    weaponLogic.bulletsRemaining -= 1;
                    weaponLogic.ReduceBulletUi();
                    reduceActiveWeaponMunition("magazineSize");
                    Shoot();
                    shotTime = Time.time;
                }
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
        if (Input.GetKeyDown("q"))
        {
            meleeAttack();
        }
        if (Input.GetKeyDown("2"))
        {
            if (ReferenceEquals(secondGun, getEquipedGun()))
            {
                switchWeapon();
            }
        }
        if (Input.GetKeyDown("3"))
        {
            if (ReferenceEquals(firstGun, getEquipedGun()))
            {
                switchWeapon();
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switchWeapon();
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("m_fight_attack_A") && !anim.IsInTransition(0))
        {
            meleeArea.SetActive(false);
        }
    }
    private void meleeAttack()
    {
        if (!melee)
        {
            meleeDamage = 20;
        }
        anim.SetTrigger("melee");
        meleeArea.SetActive(true);
        meleeArea.GetComponent<MeleeAttack>().setDamage(meleeDamage);
        

    }
    private void switchWeapon()
    {
        if (!ReferenceEquals(secondGun, null) && !weaponLogic.reloading)
        {
            if (firstWeapon)
            {
                
                firstGun.weapon.SetActive(false);
                secondGun.weapon.SetActive(true);
                melee = secondGun.meleeWeapon;
                weaponLogic.bulletsRemaining = secondGun.magazineSize;
                weaponLogic.magazinesRemaining = secondGun.magazines;
                if (!melee)
                {
                    weaponLogic.UpdateAmmoUi();
                }
                else
                {
                    weaponLogic.removeUi();
                }
                firstWeapon = false;
            }
            else
            {
                secondGun.weapon.SetActive(false);
                firstGun.weapon.SetActive(true);
                melee = firstGun.meleeWeapon;
                weaponLogic.bulletsRemaining = firstGun.magazineSize;
                weaponLogic.magazinesRemaining = firstGun.magazines;
                if (!melee)
                {
                    weaponLogic.UpdateAmmoUi();
                }
                else
                {
                    weaponLogic.removeUi();
                }
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
        if (melee)
        {
            meleeAttack();
        }
        else
        {
            float randSpread = Random.Range((float)-bulletSpread, (float)bulletSpread);
            Quaternion spread = Quaternion.Euler(0, 0 + randSpread, 0);
            Transform playerBullet = Instantiate(getEquipedGun().bullet.transform, bulletSpawner.transform.position, bulletSpawner.transform.rotation * spread);
            Bullet bulletScript = playerBullet.GetComponent<Bullet>();
            bulletScript.setDamage(getEquipedGun().damage);
        }
    }

    private void AmmoTracker()
    {
        if (weaponLogic.bulletsRemaining <= 0 && weaponLogic.magazinesRemaining <= 0 && !melee)
        {
            weaponLogic.OutOfAmmoText();
        }

        if (weaponLogic.bulletsRemaining <= 0 && weaponLogic.magazinesRemaining > 0 && !melee)
        {
            weaponLogic.ReloadText();
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
    private void setWeaponStats()
    {
        if (currentWeapon.name == "w_DesertEagle")
        {
            magazineSize = 12;
            magazines = 3;
            shotCooldown = (float)0.7;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
            bulletSpread = 5;
            damage = 35;
            melee = false;
        }
        if (currentWeapon.name == "w_ak47")
        {
            magazineSize = 30;
            magazines = 3;
            shotCooldown = (float)0.3;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
            bulletSpread = 15;
            damage = 25;
            melee = false;
        }
        if (currentWeapon.name == "w_python")
        {
            magazineSize = 6;
            magazines = 3;
            shotCooldown = (float)0.6;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
            bulletSpread = 5;
            damage = 40;
            melee = false;
        }
        if (currentWeapon.name == "w_mac10")
        {
            magazineSize = 32;
            magazines = 3;
            shotCooldown = (float)0.1;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
            bulletSpread = 20;
            damage = 15;
            melee = false;
        }
        if (currentWeapon.name == "w_rpg")
        {
            magazineSize = 3;
            magazines = 0;
            shotCooldown = (float)2;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/projectile_rpg") as GameObject;
            bulletSpread = 1;
            damage = 100;
            melee = false;
        }
        if (currentWeapon.name == "w_spas")
        {
            magazineSize = 12;
            magazines = 3;
            shotCooldown = (float)1;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/shotgun_shell") as GameObject;
            bulletSpread = 1;
            damage = 85;
            melee = false;
        }
        if (currentWeapon.name == "w_svd")
        {
            magazineSize = 10;
            magazines = 3;
            shotCooldown = (float)1.5;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
            bulletSpread = 1;
            damage = 85;
            melee = false;
        }
        if (currentWeapon.name == "w_p90")
        {
            magazineSize = 50;
            magazines = 3;
            shotCooldown = (float)0.1;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
            bulletSpread = 30;
            damage = 20;
            melee = false;
        }
        if (currentWeapon.name == "w_m4_custom")
        {
            magazineSize = 30;
            magazines = 3;
            shotCooldown = (float)0.3;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
            bulletSpread = 15;
            damage = 35;
            melee = false;
        }
        if (currentWeapon.name == "w_aug")
        {
            magazineSize = 30;
            magazines = 3;
            shotCooldown = (float)0.3;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
            bulletSpread = 10;
            damage = 35;
            melee = false;
        }
        if (currentWeapon.name == "w_scar")
        {
            magazineSize = 20;
            magazines = 3;
            shotCooldown = (float)0.3;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
            bulletSpread = 10;
            damage = 40;
            melee = false;
        }
        if (currentWeapon.name == "w_awp")
        {
            magazineSize = 10;
            magazines = 3;
            shotCooldown = (float)1.5;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
            bulletSpread = 1;
            damage = 100;
            melee = false;
        }
        if (currentWeapon.name == "w_twobarrel")
        {
            magazineSize = 2;
            magazines = 5;
            shotCooldown = (float)0.3;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
            bulletSpread = 1;
            damage = 90;
            melee = false;
        }
        if (currentWeapon.name == "w_m249")
        {
            magazineSize = 100;
            magazines = 1;
            shotCooldown = (float)0.1;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/rifle_shell") as GameObject;
            bulletSpread = 20;
            damage = 15;
            melee = false;
        }
        if (currentWeapon.name == "w_rambo_knife")
        {
            magazineSize = 0;
            magazines = 0;
            shotCooldown = (float)2;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            meleeDamage = 100;
            melee = true;

        }
        if (currentWeapon.name == "w_policebatton")
        {
            magazineSize = 0;
            magazines = 0;
            shotCooldown = (float)2;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            meleeDamage = 100;
            melee = true;

        }
        
    }
}
