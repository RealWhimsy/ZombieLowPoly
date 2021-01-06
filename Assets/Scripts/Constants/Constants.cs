
public static class Const
{

    public const int FirstWeaponIndex = 0;
    public const int SecondWeaponIndex = 1;
    public const int MaxWeaponIndex = 1;
    public const int MaxNumWeapons = MaxWeaponIndex + 1;
    public static class Events
    {
        public const string ShotFired = "SHOT_FIRED";
        public const string WeaponSwapped = "WEAPON_SWAPPED";
        public const string WeaponPickedUp = "WEAPON_PICKED_UP";
    }

    public static class WeaponNames
    {
        public const string Deagle = "w_DesertEagle";
        public const string Ak47 = "w_ak47";
    }
}
