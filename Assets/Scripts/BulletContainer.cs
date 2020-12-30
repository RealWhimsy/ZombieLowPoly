using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletContainer : MonoBehaviour
{
    bool usedGun = false;
    int magazines, magazineSize;

    public void SetValues(bool usedGun, int magazines, int magazineSize)
    {
        this.usedGun = usedGun;
        this.magazines = magazines;
        this.magazineSize = magazineSize;
    }
    public bool GetUsedGun() { return this.usedGun; }
    public int GetMagazines() { return this.magazines; }
    public int GetMagazineSize() { return this.magazineSize; }
}
