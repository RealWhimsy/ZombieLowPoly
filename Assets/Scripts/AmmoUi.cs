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
    private int grenades;

    private GameObject player;
    private PlayerManager playerManager;
    private int magazinesRemaining;
    private int bulletsRemaining;
    private int extraMags;


    private void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        player = GameObject.FindGameObjectWithTag("Player");
        ammoInfoText = GameObject.Find("MunitionText").GetComponent<Text>();
        playerManager = player.GetComponent<PlayerManager>();
        UpdateAmmoUi();
        EventManager.StartListening(Const.Events.WeaponPickedUp, UpdateAmmoUi);
        EventManager.StartListening(Const.Events.WeaponSwapped, UpdateAmmoUi);
        EventManager.StartListening(Const.Events.UpdateAmmoUi, UpdateAmmoUi);
    }

    public void UpdateAmmoUi()
    {
        bulletsRemaining = playerManager.GetActiveWeapon().ShotsInCurrentMag;
        magazinesRemaining = playerManager.GetActiveWeapon().Magazines;
        grenades = playerManager.grenades;
        
        List<GameObject> renderedBullets = GetBullets();
        List<GameObject> renderedMags = GetMags();
        List<GameObject> renderedGrenades = GetGrenades();

        foreach (GameObject bullet in renderedBullets)
        {
            Destroy(bullet);
        }
        foreach (GameObject mag in renderedMags)
        {
            Destroy(mag);
        }
        foreach (GameObject grenade in renderedGrenades)
        {
            Destroy(grenade);
        }

        InitBulletUi(grenades, magazinesRemaining);

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
        if (!reloading)
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

    public void ReduceAmmoUi(string uiTag)
    {
        for (var i = 1; i < canvas.transform.childCount; i++)
        {
            GameObject lastUiElement = canvas.transform.GetChild(canvas.transform.childCount - i).gameObject;
            if (lastUiElement.CompareTag(uiTag))
            {
                Destroy(lastUiElement);
                break;
            }
        }
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
        List<GameObject> renderedMags = GetMags();

        removeTextUi();
        foreach (GameObject mag in renderedMags)
        {
            Destroy(mag);
        }

    }

    private List<GameObject> GetBullets()
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            if (canvas.transform.GetChild(i).gameObject.CompareTag(Const.Tags.BulletSprite))
            {
                result.Add(canvas.transform.GetChild(i).gameObject);
            }
        }

        return result;
    }

    private List<GameObject> GetMags()
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            if (canvas.transform.GetChild(i).gameObject.tag == Const.Tags.MagazineSprite)
            {
                result.Add(canvas.transform.GetChild(i).gameObject);
            }
        }
        return result;
    }

    private List<GameObject> GetGrenades()
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            if (canvas.transform.GetChild(i).gameObject.tag == Const.Tags.GrenadeSprite)
            {
                result.Add(canvas.transform.GetChild(i).gameObject);
            }
        }
        return result;
    }

    public void InitBulletUi(int grenades, int magazinesRemaining)
    {
        removeTextUi();
        GameObject bulletUi = (GameObject) Resources.Load(Const.UI.BulletUiSprite, typeof(GameObject));
        GameObject magUi = (GameObject)Resources.Load(Const.UI.MagazineUiSprite, typeof(GameObject));
        GameObject grenadeUi = (GameObject)Resources.Load(Const.UI.GrenadeUiSprite, typeof(GameObject));
        double height = -107;
        int p = 0;
        int j = 0;

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
        for (int i = 0; i < magazinesRemaining + grenades; i++)
        {
            if(i < magazinesRemaining){
            GameObject mag = Instantiate(magUi, magUi.transform.position + new Vector3(i * 15, 0, 0), Quaternion.identity);
            mag.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            }
            else{
                j += 10;
                GameObject grenade = Instantiate(grenadeUi, grenadeUi.transform.position + new Vector3(i * 15 + j, 0, 0), Quaternion.identity);
                grenade.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            }
        }
    }
}