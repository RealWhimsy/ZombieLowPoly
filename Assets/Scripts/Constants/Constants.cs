using System;

public static class Const
{

    public const int FIRST_WEAPON_INDEX = 0;
    public const int SECOND_WEAPON_INDEX = 1;
    public const int MAX_WEAPON_INDEX = 1;
    public const int MAX_NUM_WEAPONS = MAX_WEAPON_INDEX + 1;
    public static class Events
    {
        public const String SHOT_FIRED = "SHOT_FIRED";
        public const String WEAPON_SWAPPED = "WEAPON_SWAPPED";
        public const String WEAPON_PICKED_UP = "WEAPON_PICKED_UP";
    }

    public static class WeaponNames
    {
        public const String DEAGLE = "w_DesertEagle";
        public const String AK47 = "w_ak47";
    }
}
