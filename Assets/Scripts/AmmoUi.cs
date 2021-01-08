﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AmmoUi : MonoBehaviour
{
    public GameObject canvas;
    public bool reloading;
    public bool outOfAmmo;
    public bool reloaded;
    private Text ammoInfoText;

    private GameObject player;
    private PlayerManager playerManager;


    private void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        player = GameObject.FindGameObjectWithTag("Player");
        ammoInfoText = GameObject.Find("MunitionText").GetComponent<Text>();
        
        playerManager = player.GetComponent<PlayerManager>();
        UpdateAmmoUi();
        EventManager.StartListening(Const.Events.WeaponPickedUp, UpdateAmmoUi);
        EventManager.StartListening(Const.Events.WeaponSwapped, UpdateAmmoUi);
    }

    public void UpdateAmmoUi()
    {
        var bulletsRemaining = playerManager.GetActiveWeapon().ShotsInCurrentMag;
        var magazinesRemaining = playerManager.GetActiveWeapon().Magazines;
        
        List<GameObject> renderedBullets = GetBullets();

        if (renderedBullets.Count != bulletsRemaining)
        {
            foreach (GameObject bullet in renderedBullets)
            {
                Destroy(bullet);
            }

            InitBulletUi();
        }

        if (bulletsRemaining > 0 || magazinesRemaining > 0)
        {
            outOfAmmo = false;
        }

        if (bulletsRemaining <= 0 && magazinesRemaining <= 0)
        {
            removeTextUi();
            ammoInfoText.text = "out of ammunition";
        }

        if (bulletsRemaining <= 0 && magazinesRemaining > 0)
        {
            removeTextUi();
            ammoInfoText.text = "press R to reload";
        }
    }

    public IEnumerator Reload(int magazineSize)
    {
        if (playerManager.GetActiveWeapon().Magazines > 0 && !reloading)
        {
            removeTextUi();
            reloading = true;
            ammoInfoText.text = "reloading...";
            yield return new WaitForSecondsRealtime(2); // TODO CONSTANTS!!!!
            ammoInfoText.text = "";
            UpdateAmmoUi();
            reloading = false;
        }
    }

    public void ReloadText()
    {
        if (!reloaded && !reloading)
        {
            removeTextUi();
            ammoInfoText.text = "press R to reload";
            reloaded = true;
        }
    }

    public void OutOfAmmoText()
    {
        if (!outOfAmmo)
        {
            removeTextUi();
            ammoInfoText.text = "out of ammunition";
            outOfAmmo = true;
        }
    }

    public void ReduceBulletUi()
    {
        for (int i = 1; i < canvas.transform.childCount; i++)
        {
            GameObject lastBullet = canvas.transform.GetChild(canvas.transform.childCount - i).gameObject;
            if (lastBullet.tag == "BulletSprite")
            {
                Destroy(lastBullet);
                break;
            }
        }

        reloaded = false;
    }

    private void removeTextUi()
    {
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            if (canvas.transform.GetChild(i).CompareTag("ReloadingStateSprite"))
            {
                canvas.transform.GetChild(i).gameObject.GetComponent<Text>().text = "";
            }
        }
    }


    public void removeUi()
    {
        List<GameObject> renderedBullets = GetBullets();
        removeTextUi();
        foreach (GameObject bullet in renderedBullets)
        {
            Destroy(bullet);
        }
    }

    private List<GameObject> GetBullets()
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            if (canvas.transform.GetChild(i).gameObject.CompareTag("BulletSprite"))
            {
                result.Add(canvas.transform.GetChild(i).gameObject);
            }
        }

        return result;
    }

    public void InitBulletUi()
    {
        removeTextUi();
        GameObject bulletUi = (GameObject) Resources.Load("Prefabs/BulletSprite", typeof(GameObject));
        double height = -107;
        int p = 0;

        for (int i = 0; i < playerManager.GetActiveWeapon().ShotsInCurrentMag; i++)
        {
            if (i % 20 == 0 && i != 0)
            {
                height += 25.6;
                p = 0;
            }

            GameObject bullet =
                Instantiate(bulletUi, new Vector3(-293 + p * 9, (float) height, 0), Quaternion.identity);
            bullet.transform.SetParent(canvas.transform, false);
            p++;
        }
    }
}