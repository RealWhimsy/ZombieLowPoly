using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Gun : MonoBehaviour {

    public int bulletsRemaining;
    public int magazinesRemaining;
    public GameObject canvas;
    public bool reloading = false;
    public bool outOfAmmo = false;
    public bool reloaded = false;
    public Text reloadAmmoTextField;
    // Start is called before the first frame update

    public void UpdateAmmoUi()
    {
        List<GameObject> renderedBullets = GetBullets();

        if (renderedBullets.Count != bulletsRemaining)
        {
            foreach (GameObject bullet in renderedBullets)
            {
                Destroy(bullet);
            }
            initBulletUi();
        }
        if(bulletsRemaining > 0 || magazinesRemaining > 0) { outOfAmmo = false; }
        if (bulletsRemaining <= 0 && magazinesRemaining <= 0)
        {
            removeTextUi();
            Text outOfAmmoText = GameObject.Find("MunitionText").GetComponent<Text>();
            outOfAmmoText.text = "out of ammunition";
        }

        if (bulletsRemaining <= 0 && magazinesRemaining > 0)
        {
            removeTextUi();
            Text reloadAmmoText = GameObject.Find("MunitionText").GetComponent<Text>();
            reloadAmmoText.text = "press R to reload";
        }
    }

    public IEnumerator Reload(int magazineSize)
    {
        if (magazinesRemaining > 0 && !reloading)
        {
            removeTextUi();
            reloading = true;
            Text reloadText = GameObject.Find("MunitionText").GetComponent<Text>();
            reloadText.text = "reloading...";
            yield return new WaitForSecondsRealtime(2);
            reloadText.text = "";
            magazinesRemaining -= 1;
            bulletsRemaining = magazineSize;
            magazinesRemaining -= 1;
            UpdateAmmoUi();
            reloading = false;
        }
    }

    public void ReloadText()
    {
        if (!reloaded && !reloading)
        {
            removeTextUi();
            Text reloadAmmoText = GameObject.Find("MunitionText").GetComponent<Text>();
            reloadAmmoText.text = "press R to reload";
            reloaded = true;
        }
    }
    public void OutOfAmmoText()
    {
        if (!outOfAmmo)
        {
            removeTextUi();
            Text outOfAmmoText = GameObject.Find("MunitionText").GetComponent<Text>();
            outOfAmmoText.text = "out of ammunition";
            outOfAmmo = true;
        }
    }
    public void ReduceBulletUi()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");

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
            if (canvas.transform.GetChild(i).tag == "ReloadingStateSprite")
            {
                canvas.transform.GetChild(i).gameObject.GetComponent<Text>().text = "";
            }
        }
    }

    private void removeTextUi()
    {
        for (int i=0; i< canvas.transform.childCount; i++)
        {
            if(canvas.transform.GetChild(i).tag == "ReloadingStateSprite")
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
            if (canvas.transform.GetChild(i).gameObject.tag == "BulletSprite")
            {
                result.Add(canvas.transform.GetChild(i).gameObject);
            }
        }
        return result;
    }

    public void initBulletUi()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        removeTextUi();
        GameObject bulletUi = (GameObject)Resources.Load("Prefabs/BulletSprite", typeof(GameObject));
        double height = -107;
        int p = 0;

        for (int i = 0; i < bulletsRemaining; i++)
        {
            if (i % 20 == 0 && i != 0)
            {
                
                height += 25.6;
                p = 0;
            }
            GameObject bullet = Instantiate(bulletUi, new Vector3(-293 + p * 9, (float)height, 0), Quaternion.identity);
            bullet.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            p++;
        }
    }
}
