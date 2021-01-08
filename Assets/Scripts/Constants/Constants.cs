
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
    }

    // Weapon names
    public static class WeaponNames
    {
        public const string Deagle = "w_DesertEagle";
        public const string Ak47 = "w_ak47";
        public const string Python = "w_python";
        public const string Aug = "w_aug";
    }
}
