using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponStats
{
    public static Dictionary<string, Weapon> weaponStatDict = new Dictionary<string, Weapon>()
    {
        [Const.WeaponNames.Deagle] = new Weapon("w_DesertEagle", 3, 12, 0.5f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 35, 5, false, WeaponType.Pistol,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Deagle_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Pistol_reload"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Pistol_shell")),

        [Const.WeaponNames.Ak47] = new Weapon("w_ak47", 2, 30, 0.15f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 25, 15, false, WeaponType.Rifle,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/AK47_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Rifle_reload_1"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Rifle_shell")),

        [Const.WeaponNames.Python] = new Weapon("w_python", 3, 6, 0.6f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 50, 5, false, WeaponType.Pistol,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Revolver_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Revolver_reload"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Pistol_shell")),

        [Const.WeaponNames.Mac10] = new Weapon("w_mac10", 3, 32, 0.07f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 15, 20, false, WeaponType.Smg,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Mac10_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/SMG_reload"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Pistol_shell")),

        [Const.WeaponNames.P90] = new Weapon("w_p90", 3, 50, 0.1f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 10, 25, false, WeaponType.Smg,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/P90_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/SMG_reload"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Pistol_shell")),

        [Const.WeaponNames.Aug] = new Weapon("w_aug", 2, 30, 0.17f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 25, 5, false, WeaponType.Rifle,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/AUG_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Rifle_reload_1"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Rifle_shell")),

        [Const.WeaponNames.Scar] = new Weapon("w_scar", 2, 25, 0.17f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 40, 5, false, WeaponType.Rifle,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Scar_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Rifle_reload_1"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Rifle_shell")),

        [Const.WeaponNames.M4] = new Weapon("w_m4_custom", 2, 30, 0.12f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 25, 13, false, WeaponType.Rifle,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/M4_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Rifle_reload_1"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Rifle_shell")),

        [Const.WeaponNames.Awp] = new Weapon("w_awp", 2, 10, 0.9f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 100, 2, false, WeaponType.Sniper,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/AWP_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Sniper_reload"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Rifle_shell")),

        [Const.WeaponNames.Svd] = new Weapon("w_svd", 2, 10, 0.4f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 80, 10, false, WeaponType.Sniper,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/SVD_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Rifle_reload_2"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Rifle_shell")),

        [Const.WeaponNames.TwoBarrel] = new Weapon("w_twobarrel", 5, 2, 0.3f,
            Resources.Load("Prefabs/shotgun_shell") as GameObject, 150, 15, false, WeaponType.Shotgun,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/twobarrel_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Shotgun_reload"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Shotgun_shell")),

        [Const.WeaponNames.Spas] = new Weapon("w_spas", 2, 12, 0.6f,
            Resources.Load("Prefabs/shotgun_shell") as GameObject, 150, 15, false, WeaponType.Shotgun,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Spas_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Shotgun_reload"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Shotgun_shell")),

        [Const.WeaponNames.M249] = new Weapon("w_m249", 1, 100, 0.1f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 20, 15, false, WeaponType.Lmg,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/M249_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Rifle_reload_2"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Rifle_shell")),

        [Const.WeaponNames.Rpg] = new Weapon("w_rpg", 5, 1, 0.4f,
            Resources.Load("Prefabs/projectile_rpg") as GameObject, 200, 5, false, WeaponType.Rpg,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/RPG_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/RPG_reload"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/none")),

        [Const.WeaponNames.RamboKnife] = new Weapon("w_rambo_knife", 0, 0, 0.5f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 70, 1, true, WeaponType.Melee,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/AK47_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Rifle_reload_1"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Rifle_shell")),

        [Const.WeaponNames.PoliceBat] = new Weapon("w_policebatton", 0, 0, 0.5f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 70, 1, true, WeaponType.Melee,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/AK47_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Rifle_reload_1"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Rifle_shell")),
        
        [Const.WeaponNames.UMP45] = new Weapon("w_ump45", 3, 25, 0.17f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 20, 15, false, WeaponType.Smg,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/P90_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/SMG_reload"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Pistol_shell")),
    };
}