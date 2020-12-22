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
    private GameObject currentWeapon;
    private GameObject weaponHand;

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
        setWeaponStats();
        mainCamera = FindObjectOfType<Camera>();
        playerManager = GetComponent<PlayerManager>();
        weaponLogic.bulletsRemaining = magazineSize;
        weaponLogic.magazinesRemaining = magazines;
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
            onWeaponChange();
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

    private void MouseMovement() {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if(groundPlane.Raycast(cameraRay, out rayLength)) {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (weaponLogic.bulletsRemaining > 0 && (Time.time - shotTime > shotCooldown))
            {
                weaponLogic.bulletsRemaining -= 1;
                weaponLogic.ReduceBulletUi();
                Shoot();
                shotTime = Time.time;
            }

        }
        if (Input.GetKeyDown("r"))
        {
            if (magazines > 0)
            {
                StartCoroutine(weaponLogic.Reload(magazineSize));
            }
        }
        //Test Weapon change until weapon pickup is implemented
        if (Input.GetKeyDown("t"))
        {
            weaponHand.transform.Find("w_ak47").gameObject.SetActive(true);
            weaponHand.transform.Find("w_DesertEagle").gameObject.SetActive(false);
        }
    }
    void Shoot()
    {
        Transform playerBullet = Instantiate(bullet.transform, bulletSpawner.transform.position, bulletSpawner.transform.rotation);
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

    private void onWeaponChange()
    {
        if (!weaponHand.transform.Find(currentWeapon.name).gameObject.activeSelf)
        {
            currentWeapon = getActiveWeapon();
            setWeaponStats();
            weaponLogic.UpdateAmmoUi();


        }
    }

    private void setWeaponStats()
    {
        if(currentWeapon.name == "w_DesertEagle")
        {
            magazineSize = 9;
            magazines = 3;
            shotCooldown = (float)0.7;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/Bullet") as GameObject;
        }
        if (currentWeapon.name == "w_ak47")
        {
            magazineSize = 25;
            magazines = 2;
            shotCooldown = (float)0.3;
            weaponLogic.bulletsRemaining = magazineSize;
            weaponLogic.magazinesRemaining = magazines;
            bullet = Resources.Load("Prefabs/Bullet") as GameObject;
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
}
