using UnityEngine;

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
    public const string VideoAssetsPath = "./Assets/VideoPlayer/";
    public const string ServerURL = "http://s715714060.online.de/";

    // Event names
    public static class Events
    {
        public const string ShotFired = "SHOT_FIRED";
        public const string ShotHitDDAZone = "SHOT_HIT_DDA_ZONE";
        public const string GrenadeThrown = "GRENADE_THROWN";
        public const string GrenadeHit = "GRENADE_HIT";
        public const string WeaponSwapped = "WEAPON_SWAPPED";
        public const string WeaponPickedUp = "WEAPON_PICKED_UP";
        public const string MeleeAttack = "MELEE_ATTACK";
        public const string MeleeEquipped = "MELEE_EUQIPPED";
        public const string GunEquipped = "GUN_EUQIPPED";

        public const string ResumeSpawningZombies = "RESUME_SPAWNING_ZOMBIES";
        public const string StopSpawningZombies = "STOP_SPAWNING_ZOMBIES";
        public const string ZombieSpawned = "ZOMBIE_SPAWNED";
        public const string ZombieKilled = "ZOMBIE_KILLED";
        public const string WaveCompleted = "WAVE_COMPLETED";
        public const string WaveStarted = "WAVE_STARTED";
        public const string LevelCompleted = "LEVEL_COMPLETED";
        public const string LevelLoaded = "LEVEL_LOADED";
        public const string InteractibleCollected = "INTERACTIBLE_COLLECTED";
        public const string UpdateAmmoUi = "UPDATE_AMMO_UI";
        public const string PlayerDead = "PLAYER_DEAD";
        public const string PlayerRespawned = "PLAYER_RESPAWNED";

        public const string DifficultySelected = "DIFFICULTY_SELECTED";
        public const string TutorialCompleted = "TUTORIAL_COMPLETED";

        public const string
            DifficultyChanged = "DIFFICULTY_CHANGED"; // called when difficulty changes between waves via DDA
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
        public const string UMP45 = "w_ump45";
        public const string Barret = "w_barret";
        public const string MP5 = "w_mp5_sil";
        public const string G36K = "w_g36k";
    }

    public static class Difficulties
    {
        public const string Dif1 = "DIFFICULTY_ONE";
        public const string Dif2 = "DIFFICULTY_TWO";
        public const string Dif3 = "DIFFICULTY_THREE";
        public const string Dif4 = "DIFFICULTY_FOUR";
        public const string Dif5 = "DIFFICULTY_FIVE";
        public const string Dif6 = "DIFFICULTY_SIX";
        public const string Dif7 = "DIFFICULTY_SEVEN";
        public const string Dif8 = "DIFFICULTY_EIGHT";
        public const string Dif9 = "DIFFICULTY_NINE";
        public const string Dif10 = "DIFFICULTY_TEN";
        public const int MinDifficultyIndex = 0;
        public const int MaxDifficultyIndex = 9;
        public const int MaxDifficultyChangePerWave = 4;
    }

    public static class Grenade
    {
        public const int GrenadeDamage = 100;
        public const int MaxGrenades = 5;
        public const string GrenadePrefab = "Prefabs/Grenade";
        public const string GrenadeExplosion = "Prefabs/grenade_explosion";
        public const int StartGrenades = 2;
    }

    public static class Shotgun
    {
        public const int ShotgunSplinters = 6;
    }

    public static class WeaponTypes
    {
        public const string Pistol = "Pistol";
        public const string Rifle = "Rifle";
        public const string Shotgun = "Shotgun";
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
        public const string MeleeAttack = "Sounds_Ingame/Weapons/melee";
        public const string Wave = "Sounds_Ingame/Wave/wave";
        public const string Death = "Sounds_Ingame/Death/SCREAM3";
    }

    public static class SceneNames
    {
        public const string Forest = "Playground";
        public const string Desert = "Desert";
        public const string PirateBay = "Pirate Bay";
        public const string City = "CIty";
        public const string CutsceneIntro = "CutsceneIntro";
        public const string Tutorial = "Tutorial";
        public const string LevelCompletedScene = "LevelCompleted";
        public const string EndScene = "EndScene";
    }

    public static class UI
    {
        public const string BulletUiSprite = "Prefabs/BulletSprite";
        public const string MagazineUiSprite = "Prefabs/MagazineSprite";
        public const string GrenadeUiSprite = "Prefabs/GrenadeSprite";
        public const string HUDCanvas = "Prefabs/HUD";
    }

    public static class Player
    {
        public const int RespawnTime = 5;
    }

    public class Magazines
    {
        public const int MaxMagazines = 5;
    }

    public static class PhpVariables
    {
        public const string IsFirstLogCall = "isFirstLogCall";
        public const string Id = "id";
        public const string Message = "message";
        public const string Tag = "tag";
        public const string ShotsFired = "shotsFired";
        public const string ShotsHit = "shotsHit";
        public const string DamageTaken = "damageTaken";
        public const string GrenadesThrown = "grenadesThrown";
        public const string GrenadesHit = "grenadesHit";
        public const string TimeSpent = "timeSpent";
        public const string DeathCount = "deathCount";
        public const string InteractiblesCollected = "interactiblesCollected";
        public const string MeleeAttacks = "meleeAttacks";
        public const string DifficultyIndex = "difficultyIndex";
        public const string TriesForLevel = "triesForLevel";
    }

    public static class Tutorial
    {
        public static readonly Vector3 GrenadeBoxOneVector = new Vector3(47.938255f, 0.17f, 19.60897f);
        public static readonly Vector3 GrenadeBoxTwoVector = new Vector3(47.938255f, 0.17f, 22.01942f);
        public static readonly Vector3 GrenadeBoxThreeVector = new Vector3(47.938255f, 0.17f, 16.93942f);
    }
}