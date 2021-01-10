using System.Collections;
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
    private int granades;

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
        List<GameObject> renderedMags = GetMags();

        if (renderedBullets.Count != bulletsRemaining)
        {
            foreach (GameObject bullet in renderedBullets)
            {
                Destroy(bullet);
            }
            foreach (GameObject mag in renderedMags)
            {
                Destroy(mag);
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
    public void ReduceGranadeUi(){

        for (int i = 1; i < canvas.transform.childCount; i++)
        {
            GameObject lastGranade = canvas.transform.GetChild(canvas.transform.childCount - i).gameObject;
            if (lastGranade.tag == "GranadeSprite")
            {
                Destroy(lastGranade);
                break;
            }
        }
    }

    public void ReduceMagUi()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");

        for (int i = 1; i < canvas.transform.childCount; i++)
        {
            GameObject lastBullet = canvas.transform.GetChild(canvas.transform.childCount - i).gameObject;
            if (lastBullet.tag == "MagazineSprite")
            {
                Destroy(lastBullet);
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
            if (canvas.transform.GetChild(i).gameObject.CompareTag("BulletSprite"))
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
            if (canvas.transform.GetChild(i).gameObject.tag == "MagazineSprite")
            {
                result.Add(canvas.transform.GetChild(i).gameObject);
            }
        }
        return result;
    }

    public void InitBulletUi()
    {
        granades = playerManager.granades;
        var magazinesRemaining = playerManager.GetActiveWeapon().Magazines;
        removeTextUi();
        GameObject bulletUi = (GameObject) Resources.Load("Prefabs/BulletSprite", typeof(GameObject));
        GameObject magUi = (GameObject)Resources.Load("Prefabs/MagazineSprite", typeof(GameObject));
        GameObject granadeUi = (GameObject)Resources.Load("Prefabs/GranadeSprite", typeof(GameObject));
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
        for (int i = 0; i < magazinesRemaining + granades; i++)
        {
            if(i < magazinesRemaining){
            GameObject mag = Instantiate(magUi, magUi.transform.position + new Vector3(i * 15, 0, 0), Quaternion.identity);
            mag.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            }
            else{
                j += 10;
                GameObject granade = Instantiate(granadeUi, granadeUi.transform.position + new Vector3(i * 15 + j, 0, 0), Quaternion.identity);
                granade.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            }
        }
    }
}