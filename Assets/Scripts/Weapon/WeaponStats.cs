using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponStats
{
    public static Dictionary<string, Weapon> weaponStatDict = new Dictionary<string, Weapon>()
    {
        [Const.WeaponNames.Deagle] = new Weapon("w_DesertEagle", 3, 12, 0.6f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 35, 5, false,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Deagle_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Pistol_reload"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Pistol_shell")),

        [Const.WeaponNames.Ak47] = new Weapon("w_ak47", 3, 30, 0.2f,
            Resources.Load("Prefabs/rifle_shell") as GameObject, 25, 15, false,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/AK47_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Rifle_reload_1"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Rifle_shell")),

        [Const.WeaponNames.Python] = new Weapon("w_python", 3, 6, 0.6f, 
            Resources.Load("Prefabs/rifle_shell") as GameObject, 40, 5, false,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Revolver_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Revolver_reload"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Pistol_shell")),
        
        [Const.WeaponNames.Aug] = new Weapon("w_aug", 3, 6, 0.6f, 
            Resources.Load("Prefabs/rifle_shell") as GameObject, 40, 5, false,
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/AK47_shot"),
            (AudioClip) Resources.Load("Sounds_Ingame/Weapons/Rifle_reload_1"),
            (AudioClip) Resources.Load("Sounds_Ingame/Bullets/Rifle_shell")),
    };
}