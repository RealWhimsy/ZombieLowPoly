
using System;
using System.Linq;

public class Difficulty
{
    private int zombiesForWave;
    private int zombieHealth;
    private int zombieDamage;
    private float zombieSpeed;
    private float timeBetweenSpawns;
    private int stage;

    private static Difficulty _currentDifficulty;
    private static int _currentDifficultyIndex = 4;

    public Difficulty(int stage, int zombiesForWave, int zombieHealth, int zombieDamage, float zombieSpeed, float timeBetweenSpawns)
    {
        this.stage = stage;
        this.zombiesForWave = zombiesForWave;
        this.zombieHealth = zombieHealth;
        this.zombieDamage = zombieDamage;
        this.zombieSpeed = zombieSpeed;
        this.timeBetweenSpawns = timeBetweenSpawns;
    }

    public static void SetDifficultyToIndex(int index)
    {
        if (index > Const.Difficulties.MaxDifficultyIndex)
        {
            index = Const.Difficulties.MaxDifficultyIndex;
        } 
        else if (index < Const.Difficulties.MinDifficultyIndex)
        {
            index = Const.Difficulties.MinDifficultyIndex;
        }

        _currentDifficultyIndex = index;
        CurrentDifficulty = DifficultyStats.difficulties.ElementAt(index).Value;
    }
    
    public static void ApplyDifficultyChange()
    {
        if (_currentDifficultyIndex > Const.Difficulties.MaxDifficultyIndex)
        {
            _currentDifficultyIndex = Const.Difficulties.MaxDifficultyIndex;
        } 
        else if (_currentDifficultyIndex < Const.Difficulties.MinDifficultyIndex)
        {
            _currentDifficultyIndex = Const.Difficulties.MinDifficultyIndex;
        }
        
        CurrentDifficulty = DifficultyStats.difficulties.ElementAt(_currentDifficultyIndex).Value;
    }

    public int Stage
    {
        get => stage;
        set => stage = value;
    }

    public int ZombiesForWave => zombiesForWave;

    public int ZombieHealth => zombieHealth;

    public int ZombieDamage => zombieDamage;

    public float ZombieSpeed => zombieSpeed;

    public float TimeBetweenSpawns => timeBetweenSpawns;

    public static Difficulty CurrentDifficulty
    {
        get => _currentDifficulty;
        set => _currentDifficulty = value;
    }

    public static int CurrentDifficultyIndex
    {
        get => _currentDifficultyIndex;
        set
        {
            if (value > Const.Difficulties.MaxDifficultyIndex)
            {
                value = Const.Difficulties.MaxDifficultyIndex;
            }
            _currentDifficultyIndex = value;
        }
        
    }
}