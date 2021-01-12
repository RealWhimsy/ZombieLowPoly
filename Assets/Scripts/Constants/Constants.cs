public static class Const
{
    /******************************************************************
     * Constants in C# are named in PascalCase, not in SCREAMING_CAPS *
     ******************************************************************/

    // General Constants
    public const int FirstWeaponIndex = 0;
    public const int SecondWeaponIndex = 1;
    public const int MaxWeaponIndex = 1;
    public const int MaxNumWeapons = MaxWeaponIndex + 1;

    // Event names
    public static class Events
    {
        public const string ShotFired = "SHOT_FIRED";
        public const string WeaponSwapped = "WEAPON_SWAPPED";
        public const string WeaponPickedUp = "WEAPON_PICKED_UP";
        public const string MeleeAttack = "MELEE_ATTACK";

        public const string ResumeSpawningZombies = "RESUME_SPAWNING_ZOMBIES";
        public const string StopSpawningZombies = "STOP_SPAWNING_ZOMBIES";
        public const string ZombieSpawned = "ZOMBIE_SPAWNED";
        public const string ZombieKilled = "ZOMBIE_KILLED";
        public const string WaveCompleted = "WAVE_COMPLETED";
        public const string WavePassed = "WAVE_PASSED";
        public const string LevelCompleted = "LEVEL_COMPLETED";
    }

    // Weapon names
    public static class WeaponNames
    {
        public const string Deagle = "w_DesertEagle";
        public const string Ak47 = "w_ak47";
        public const string Python = "w_python";
        public const string Mac10 = "w_mac10";
        public const string P90 = "w_p90";
        public const string Aug = "w_aug";
        public const string Scar = "w_scar";
        public const string M4 = "w_m4_custom";
        public const string Awp = "w_awp";
        public const string Svd = "w_svd";
        public const string TwoBarrel = "w_twobarrel";
        public const string Spas = "w_spas";
        public const string M249 = "w_m249";
        public const string Rpg = "w_rpg";
        public const string RamboKnife = "w_rambo_knife";
        public const string PoliceBat = "w_policebatton";
    }


    public static class WeaponTypes
    {
        public const string Pistol = "Pistol";
        public const string Rifle = "Rifle";
        public const string Melee = "Melee";
        public const string Rpg = "Rpg";
    }


    public static class SFX
    {
        public const string WeaponPickup = "Sounds_Ingame/Using/Weapon_switch";
        public const string AmmoEmpty = "Sounds_Ingame/Using/Ammo_empty";
        public const string AmmoPickup = "Sounds_Ingame/Using/Ammo_pickup";
        public const string Explosion = "Sounds_Ingame/Weapons/Explosion";

        public static readonly string[] Hits =
        {
            "Sounds_Ingame/Hits/hit1",
            "Sounds_Ingame/Hits/hit2",
            "Sounds_Ingame/Hits/hit3",
            "Sounds_Ingame/Hits/hit4"
        };

        public static readonly string[] Zombies =
        {
            "Sounds_Ingame/Zombies/z1",
            "Sounds_Ingame/Zombies/z2",
            "Sounds_Ingame/Zombies/z3",
            "Sounds_Ingame/Zombies/z4",
            "Sounds_Ingame/Zombies/z5",
            "Sounds_Ingame/Zombies/z6",
            "Sounds_Ingame/Zombies/z7",
            "Sounds_Ingame/Zombies/z8"
        };

        public static readonly string[] Steps =
        {
            "Sounds_Ingame/Steps/s1",
            "Sounds_Ingame/Steps/s2",
            "Sounds_Ingame/Steps/s3",
            "Sounds_Ingame/Steps/s4",
            "Sounds_Ingame/Steps/s5"
        };
    }

    public static class SceneNames
    {
        public const string Forest = "Playground";
    }
}