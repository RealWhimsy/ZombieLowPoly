﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    private GameObject weapon;
    private int magazines;
    private int shotsInCurrentMag;
    private int maxMagazineSize;
    private int maxMagazines;
    private int damage;

    private String name;

    private float shotCooldown;
    private float maxBulletSpread;

    private bool meleeWeapon;
    private WeaponType weaponType;
    
    private GameObject bullet;
    private AudioClip shotSound;
    private AudioClip reloadSound;
    private AudioClip shellSound;

    public Weapon(String name, int magazines, int shotsInCurrentMag, float shotCooldown, GameObject bullet, int damage,
        float maxBulletSpread, bool meleeWeapon, WeaponType weaponType, AudioClip shotSound, AudioClip reloadSound, AudioClip shellSound)
    {
        this.name = name;
        this.magazines = magazines;
        this.shotsInCurrentMag = shotsInCurrentMag;
        this.maxMagazines = magazines;
        this.maxMagazineSize = shotsInCurrentMag;
        this.shotCooldown = shotCooldown;
        this.bullet = bullet;
        this.damage = damage;
        this.maxBulletSpread = maxBulletSpread;
        this.meleeWeapon = meleeWeapon;
        this.weaponType = weaponType;
        
        this.shotSound = shotSound;
        this.reloadSound = reloadSound;
        this.shellSound = shellSound;
    }

    public Weapon(Weapon weapon)
    {
        magazines = weapon.magazines;
        shotsInCurrentMag = weapon.shotsInCurrentMag;
        maxMagazines = weapon.magazines;
        maxMagazineSize = weapon.shotsInCurrentMag;
        shotCooldown = weapon.shotCooldown;
        bullet = weapon.bullet;
        damage = weapon.damage;
        maxBulletSpread = weapon.maxBulletSpread;
        meleeWeapon = weapon.meleeWeapon;
        weaponType = weapon.weaponType;
        shotSound = weapon.shotSound;
        reloadSound = weapon.reloadSound;
        shellSound = weapon.shellSound;
    }

    public void Reload()
    {
        shotsInCurrentMag = maxMagazineSize;
    }

    public GameObject Weapon1
    {
        get => weapon;
        set => weapon = value;
    }

    public int Magazines
    {
        get => magazines;
        set => magazines = value;
    }

    public int ShotsInCurrentMag
    {
        get => shotsInCurrentMag;
        set => shotsInCurrentMag = value;
    }

    public int MaxMagazineSize
    {
        get => maxMagazineSize;
        set => maxMagazineSize = value;
    }

    public int MaxMagazines
    {
        get => maxMagazines;
        set => maxMagazines = value;
    }

    public float ShotCooldown
    {
        get => shotCooldown;
        set => shotCooldown = value;
    }

    public GameObject Bullet
    {
        get => bullet;
        set => bullet = value;
    }

    public int Damage
    {
        get => damage;
        set => damage = value;
    }

    public bool MeleeWeapon
    {
        get => meleeWeapon;
        set => meleeWeapon = value;
    }

    public float MaxBulletSpread
    {
        get => maxBulletSpread;
        set => maxBulletSpread = value;
    }

    public string Name
    {
        get => name;
        set => name = value;
    }

    public WeaponType WeaponType => weaponType;

    public AudioClip ShotSound => shotSound;

    public AudioClip ReloadSound => reloadSound;

    public AudioClip ShellSound => shellSound;
}