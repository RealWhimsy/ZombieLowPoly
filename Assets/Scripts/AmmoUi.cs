using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AmmoUi : MonoBehaviour
{
    public bool reloading;
    public bool outOfAmmo;
    public bool reloaded;
    private Text ammoInfoText;
    private int grenades;

    private GameObject player;
    private GameObject playerStatsBox;
    private GameObject healthbar;
    private PlayerManager playerManager;
    private int magazinesRemaining;
    private int bulletsRemaining;
    private int extraMags;


    private void Start()
    {
        playerStatsBox = GameObject.FindGameObjectWithTag("PlayerStatsUi");
        healthbar = GameObject.FindGameObjectWithTag("Healthbar");
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
        for (var i = 1; i < playerStatsBox.transform.childCount; i++)
        {
            GameObject lastUiElement = playerStatsBox.transform.GetChild(playerStatsBox.transform.childCount - i).gameObject;
            if (lastUiElement.CompareTag(uiTag))
            {
                Destroy(lastUiElement);
                break;
            }
        }
    }

    private void removeTextUi()
    {
        for (int i = 0; i < playerStatsBox.transform.childCount; i++)
        {
            if (playerStatsBox.transform.GetChild(i).CompareTag("ReloadingStateSprite"))
            {
                playerStatsBox.transform.GetChild(i).gameObject.GetComponent<Text>().text = "";
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
        for (int i = 0; i < playerStatsBox.transform.childCount; i++)
        {
            if (playerStatsBox.transform.GetChild(i).gameObject.CompareTag(Const.Tags.BulletSprite))
            {
                result.Add(playerStatsBox.transform.GetChild(i).gameObject);
            }
        }

        return result;
    }

    private List<GameObject> GetMags()
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = 0; i < playerStatsBox.transform.childCount; i++)
        {
            if (playerStatsBox.transform.GetChild(i).gameObject.tag == Const.Tags.MagazineSprite)
            {
                result.Add(playerStatsBox.transform.GetChild(i).gameObject);
            }
        }
        return result;
    }

    private List<GameObject> GetGrenades()
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = 0; i < playerStatsBox.transform.childCount; i++)
        {
            if (playerStatsBox.transform.GetChild(i).gameObject.tag == Const.Tags.GrenadeSprite)
            {
                result.Add(playerStatsBox.transform.GetChild(i).gameObject);
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
        RectTransform healthbarRT = (RectTransform)healthbar.transform;
        RectTransform playerStatsBoxRT = (RectTransform)playerStatsBox.transform;
        RectTransform bulletUiRT = (RectTransform)bulletUi.transform;
        RectTransform grenadeUiRT = (RectTransform)grenadeUi.transform;
        RectTransform magUiRT = (RectTransform)magUi.transform;
        float uiBoxWidth = playerStatsBoxRT.rect.width;
        healthbarRT.sizeDelta = new Vector2 (uiBoxWidth * 2f, uiBoxWidth * 0.32f);
        bulletUiRT.sizeDelta = new Vector2 (uiBoxWidth * 0.145f, uiBoxWidth * 0.49f);
        grenadeUiRT.sizeDelta = new Vector2 (uiBoxWidth * 0.48f, uiBoxWidth * 0.51f);
        magUiRT.sizeDelta = new Vector2 (uiBoxWidth * 0.32f, uiBoxWidth * 0.49f);
        double height = healthbar.transform.localPosition.y + healthbarRT.rect.height * 1.7;
        int p = 0;
        float j = 0;

        for (int i = 0; i < playerManager.GetActiveWeapon().ShotsInCurrentMag; i++)
        {
            if (i % 20 == 0 && i != 0)
            {
                height += bulletUiRT.rect.height;
                p = 0;
            }

            GameObject bullet =
                Instantiate(bulletUi, new Vector3(healthbar.transform.localPosition.x - healthbarRT.rect.width * 0.4f + p * bulletUiRT.rect.width * 1.2f, (float) height, 0), Quaternion.identity);
            bullet.transform.SetParent(playerStatsBox.transform, false);
            p++;
        }
        for (int i = 0; i < magazinesRemaining + grenades; i++)
        {
            if(i < magazinesRemaining){
            GameObject mag = Instantiate(magUi, new Vector3((healthbar.transform.localPosition.x + healthbarRT.rect.width * 0.7f) + i * bulletUiRT.rect.width * 2f,  healthbar.transform.localPosition.y, 0), Quaternion.identity);
            mag.transform.SetParent(playerStatsBox.transform, false);
            }
            else{
                j += grenadeUiRT.rect.width * 0.4f;
                GameObject grenade = Instantiate(grenadeUi, new Vector3((healthbar.transform.localPosition.x + healthbarRT.rect.width * 0.7f) + (i * bulletUiRT.rect.width * 2f + j),  healthbar.transform.localPosition.y, 0), Quaternion.identity);
                grenade.transform.SetParent(playerStatsBox.transform, false);
            }
        }
    }
}