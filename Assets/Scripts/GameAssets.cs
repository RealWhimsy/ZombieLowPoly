﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i {
        get {
            if(_i == null) {
                _i = Instantiate(Resources.Load("GameAssets") as GameObject).GetComponent<GameAssets>();
            }
            return _i;
        }
    }

    private System.Random random = new System.Random();

    public int GenerateRandomNumber(int min, int max)
    {
        int randInt = random.Next(min, max + 1);
        return randInt;
    }
}