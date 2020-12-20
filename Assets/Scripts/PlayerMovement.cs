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

    public GameObject bullet;
    public GameObject gun;
    public float shotCooldown;
    Gun weapon = new Gun();
    public int magazineSize = 10;
        public int magazines = 3;


    private void Start() {
        controller = GetComponent<CharacterController>();
        mainCamera = FindObjectOfType<Camera>();
        playerManager = GetComponent<PlayerManager>();
        Debug.Log(weapon.bulletsRemaining);
        weapon.bulletsRemaining = magazineSize;
        weapon.magazinesRemaining = magazines;
        weapon.initBulletUi();
    }

    private void Update() {
        if (!playerManager.isDead())
        {
            BaseMovement();
            MouseMovement();
            AmmoTracker();
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
