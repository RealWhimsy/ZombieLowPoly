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
        public const string GrenadeThrown = "GRENADE_THROWN";
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
        public const string InteractibleCollected = "INTERACTIBLE_COLLECTED";
        public const string UpdateAmmoUi = "UPDATE_AMMO_UI";
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

    public static class Grenade
    {
        public const int GrenadeDamage = 100;
        public const int MaxGrenades = 5;
        public const string GrenadePrefab = "Prefabs/Grenade"; 
    }

    public static class WeaponTypes
    {
        public const string Pistol = "Pistol";
        public const string Rifle = "Rifle";
        public const string Melee = "Melee";
        public const string Rpg = "Rpg";
    }

    public static class Tags
    {
        public const string BulletSprite = "BulletSprite";
        public const string GrenadeSprite = "GrenadeSprite";
        public const string MagazineSprite = "MagazineSprite";
    }

    public static class SFX
    {
        public const string WeaponPickup = "Sounds_Ingame/Using/Weapon_switch";
        public const string AmmoEmpty = "Sounds_Ingame/Using/Ammo_empty";
        public const string AmmoPickup = "Sounds_Ingame/Using/Ammo_pickup";
        public const string Explosion = "Sounds_Ingame/Weapons/Explosion";
        public const string FootSteps = "Sounds_Ingame/Steps/Ground_running";
        public const string Hits = "Sounds_Ingame/Hits";
        public const string Zombies = "Sounds_Ingame/Zombies";
        public const string Grenade = "Sounds_Ingame/Using/ImpactGrenade";
        public const string GrenadeThrow = "Sounds_Ingame/Using/ThrowGrenade";
        public const string MeleeAttack = "Sounds_Ingame/Weapons/knife_attack";
    }

    public static class SceneNames
    {
        public const string Forest = "Playground";
    }

    public static class UI
    {
        public const string BulletUiSprite = "Prefabs/BulletSprite";
        public const string MagazineUiSprite = "Prefabs/MagazineSprite";
        public const string GrenadeUiSprite = "Prefabs/GrenadeSprite";
    }
}
