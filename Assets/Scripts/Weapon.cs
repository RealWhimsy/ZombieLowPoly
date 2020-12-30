﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject weapon;
    public int magazines;
    public int magazineSize;
    public int maxMagazineSize;
    public int maxMagazines;
    public float shotCooldown;
    public GameObject bullet;
    public int damage;
    public bool meleeWeapon;
    public Weapon(GameObject weapon, int magazines, int magazineSize, float shotCooldown, GameObject bullet, int damage, bool meleeWeapon)
    {
        this.weapon = weapon;
        this.magazines = magazines;
        this.magazineSize = magazineSize;
        this.maxMagazines = magazines;
        this.maxMagazineSize = magazineSize;
        this.shotCooldown = shotCooldown;
        this.bullet = bullet;
        this.damage = damage;
        this.meleeWeapon = meleeWeapon;
    }
}