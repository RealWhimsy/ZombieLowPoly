using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject weapon;
    public int magazines;
    public int magazineSize;
    public int maxMagazineSize;
    public int maxMagazines;
    public Weapon(GameObject weapon, int magazines, int magazineSize)
    {
        this.weapon = weapon;
        this.magazines = magazines;
        this.magazineSize = magazineSize;
        this.maxMagazines = magazines;
        this.maxMagazineSize = magazineSize;
    }
}
