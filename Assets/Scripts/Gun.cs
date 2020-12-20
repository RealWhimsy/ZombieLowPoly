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
        canvas = GameObject.FindGameObjectWithTag("Canvas");

        List<GameObject> renderedBullets = GetBullets();

        if (renderedBullets.Count != bulletsRemaining)
        {
            foreach (GameObject bullet in renderedBullets)
            {
                Destroy(bullet);
            }
            initBulletUi();
        }
    }

    public IEnumerator Reload(int magazineSize)
    {
        if (magazinesRemaining > 0 && !reloading)
        {
            if(reloadAmmoTextField != null)
            {
                Destroy(reloadAmmoTextField);
            }
            reloading = true;
            Text reloadText = (Text)Resources.Load("Prefabs/MunitionText", typeof(Text));
            reloadText.text = "reloading...";
            Text reloadTextField = Instantiate(reloadText, reloadText.transform.position, Quaternion.identity);
            reloadTextField.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            Debug.Log("before Wait");
            yield return new WaitForSecondsRealtime(2);
            Debug.Log("after Wait");
            Destroy(reloadTextField);
            magazinesRemaining -= 1;
            bulletsRemaining = magazineSize;
            UpdateAmmoUi();
            reloading = false;
        }
    }

    public void ReloadText()
    {
        if (!reloaded)
        {
            Text reloadAmmoText = (Text)Resources.Load("Prefabs/MunitionText", typeof(Text));
            reloadAmmoText.text = "press R to reload";
            reloadAmmoTextField = Instantiate(reloadAmmoText, reloadAmmoText.transform.position, Quaternion.identity);
            reloadAmmoTextField.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            reloaded = true;
        }
    }
    public void OutOfAmmoText()
    {
        if (!outOfAmmo)
        {
            Text outOfAmmoText = (Text)Resources.Load("Prefabs/MunitionText", typeof(Text));
            outOfAmmoText.text = "out of ammunition";
            Text outOfAmmoTextField = Instantiate(outOfAmmoText, outOfAmmoText.transform.position, Quaternion.identity);
            outOfAmmoTextField.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
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

    private List<GameObject> GetBullets()
    {
        List<GameObject> result = new List<GameObject>();
        for(int i=0; i < canvas.transform.childCount; i++)
        {
            if(canvas.transform.GetChild(i).gameObject.tag == "BulletSprite")
            {
                result.Add(canvas.transform.GetChild(i).gameObject);
            }
        }
        return result;
    }

    public void initBulletUi()
    {
        GameObject bulletUi = (GameObject) Resources.Load("Prefabs/BulletSprite", typeof(GameObject));

        for (int i = 0; i < bulletsRemaining; i++)
        {

            GameObject bullet = Instantiate(bulletUi, new Vector3(-293 + i * 9, -107, 0), Quaternion.identity);
            bullet.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        }
    }
}
