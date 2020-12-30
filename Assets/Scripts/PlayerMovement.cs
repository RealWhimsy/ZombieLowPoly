using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 0;
    public Animator anim;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private PlayerManager playerManager;
    private GameObject player;
    private Gun weapon;

    public GameObject bullet;
    public GameObject gun;
    public float shotCooldown;

    public int magazineSize = 10;
    public int magazines = 3;


    private void Start()
    {
        addGunToPlayer();

        controller = GetComponent<CharacterController>();
        playerManager = GetComponent<PlayerManager>();
        Debug.Log(weapon.bulletsRemaining);
        weapon.bulletsRemaining = magazineSize;
        weapon.magazinesRemaining = magazines;
        weapon.initBulletUi();
    }

    private void addGunToPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weapon = player.AddComponent<Gun>();
    }

    private void Update()
    {
        if (!playerManager.isDead())
        {
            BaseMovement();
            MouseMovement();
            AmmoTracker();
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

    private void MouseMovement()
    {

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 
            Camera.main.transform.position.y - transform.position.y));
        Vector3 lookDirection = new Vector3(worldPos.x, transform.position.y, worldPos.z);

        transform.LookAt(lookDirection);
        Debug.DrawLine(Camera.main.transform.position, lookDirection, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            if (weapon.bulletsRemaining > 0)
            {
                weapon.bulletsRemaining -= 1;
                weapon.ReduceBulletUi();
                Shoot();
            }

        }
        if (Input.GetKeyDown("r"))
        {
            if (magazines > 0)
            {
                StartCoroutine(weapon.Reload(magazineSize));
            }
        }
    }
    void Shoot()
    {
        Transform playerBullet = Instantiate(bullet.transform, gun.transform.position, gun.transform.rotation);
        Bullet bulletScript = playerBullet.GetComponent<Bullet>();
        bulletScript.setDamage(playerManager.damage);
    }

    private void AmmoTracker()
    {
        if (weapon.bulletsRemaining <= 0 && weapon.magazinesRemaining <= 0)
        {
            weapon.OutOfAmmoText();
        }

        if (weapon.bulletsRemaining <= 0 && weapon.magazinesRemaining > 0)
        {
            weapon.ReloadText();
        }
    }

}
