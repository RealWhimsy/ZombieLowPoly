using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class DDAManager : MonoBehaviour
{

    private static int _shotsFired;
    private static int _shotsHit;
    private static int _grenadesThrown;
    private static int _grenadesHit;
    private static int _damageTaken;
    private static float _timeTaken;
    private static int _playerFeedbackDifficulty;
    
    private static DateTime startTime;
    private static DateTime endTime;
    
    
    // Start is called before the first frame update
    void Start()
    {
         EventManager.StartListening(Const.Events.ShotFired,() => _shotsFired++);
         EventManager.StartListening(Const.Events.ShotHitDDAZone, () => _shotsHit++);
         EventManager.StartListening(Const.Events.GrenadeThrown, () => _grenadesThrown++);
         EventManager.StartListening(Const.Events.GrenadeHit, () => _grenadesHit++);
         
         EventManager.StartListening(Const.Events.WaveCompleted, AdjustDDA);
         EventManager.StartListening(Const.Events.DifficultyChanged, HandleDifficultyChange);
         EventManager.StartListening(Const.Events.WaveStarted, WaveStarted);
         startTime = DateTime.Now;
    }

    private void AdjustDDA()
    {
        var difficultyChange = CalculateTotalDifficultyChange();

        Difficulty.CurrentDifficultyIndex += difficultyChange;
    }

    private int CalculateTotalDifficultyChange()
    {
        var shotScore = CalculateShotScore();
        var grenadeScore = CalculateGrenadeScore();
        var damageTakenScore = CalculateDamageTakenScore();
        var timeScore = CalculateTimeScore();
        var difficultyChange = shotScore + grenadeScore + damageTakenScore + _playerFeedbackDifficulty;

        // set to maximum difficulty change allowed for one wave
        if (difficultyChange > Const.Difficulties.MaxDifficultyChangePerWave)
        {
            difficultyChange = Const.Difficulties.MaxDifficultyChangePerWave;
        }

        if (difficultyChange < -Const.Difficulties.MaxDifficultyChangePerWave)
        {
            difficultyChange = -Const.Difficulties.MaxDifficultyChangePerWave;
        }

        return difficultyChange;
    }

    private int CalculateTimeScore()
    {
        endTime = DateTime.Now;
        TimeSpan timeSpan = endTime - startTime;
        double seconds = timeSpan.TotalSeconds;

        if (seconds < 30)
        {
            return 2;
        }

        if (seconds < 50)
        {
            return 1;
        }

        if (seconds < 70)
        {
            return 0;
        }

        if (seconds < 90)
        {
            return -1;
        }

        return -2;
    }

    private int CalculateDamageTakenScore()
    {
        if (_damageTaken == 0)
        {
            return 2;
        }

        if (_damageTaken <= 15)
        {
            return 1;
        }

        if (_damageTaken <= 30)
        {
            return 0;
        }

        if (_damageTaken <= 50)
        {
            return -1;
        }

        return -2;
    }

    private int CalculateGrenadeScore()
    {
        var grenadeHitsPerThrow = (float) _grenadesHit / _grenadesThrown;

        if (grenadeHitsPerThrow >= 4)
        {
            return 2;
        }

        if (grenadeHitsPerThrow >= 3)
        {
            return 1;
        }

        if (grenadeHitsPerThrow >= 2)
        {
            return 0;
        }

        if (grenadeHitsPerThrow >= 1)
        {
            return -1;
        }

        return -2;
    }

    private int CalculateShotScore()
    {
        var percentOfShotsHit = (float) _shotsHit / _shotsFired;
        percentOfShotsHit *= 100; // multiply by 100 to get easier to read % value

        if (percentOfShotsHit > 90)
        {
            return 2;
        }

        if (percentOfShotsHit > 66)
        {
            return 1;
        }

        if (percentOfShotsHit > 50)
        {
            return 0;
        }

        if (percentOfShotsHit > 30)
        {
            return -1;
        }

        return -2;
    }
    
    private void HandleDifficultyChange()
    {
        KeepDifficultyWithinBounds();
        ResetSingleWaveStats();
        
        // get difficulty at index from Dictionary
        Difficulty.CurrentDifficulty = DifficultyStats.difficulties.ElementAt(Difficulty.CurrentDifficultyIndex).Value;
    }

    private void WaveStarted()
    {
        startTime = DateTime.Now;
    }

    /**
     * Reset all stats that are meant to be calculated for a single wave only
     */
    private void ResetSingleWaveStats()
    {
        _damageTaken = 0;
    }

    private void KeepDifficultyWithinBounds()
    {
        if (Difficulty.CurrentDifficultyIndex < Const.Difficulties.MinDifficultyIndex)
        {
            Difficulty.CurrentDifficultyIndex = Const.Difficulties.MinDifficultyIndex;
        }

        if (Difficulty.CurrentDifficultyIndex > Const.Difficulties.MaxDifficultyIndex)
        {
            Difficulty.CurrentDifficultyIndex = Const.Difficulties.MaxDifficultyIndex;
        }
    }

    public static int ShotsFired
    {
        get => _shotsFired;
        set => _shotsFired = value;
    }

    public static int ShotsHit
    {
        get => _shotsHit;
        set => _shotsHit = value;
    }

    public static int GrenadesThrown
    {
        get => _grenadesThrown;
        set => _grenadesThrown = value;
    }

    public static int GrenadesHit
    {
        get => _grenadesHit;
        set => _grenadesHit = value;
    }

    public static int DamageTaken
    {
        get => _damageTaken;
        set => _damageTaken = value;
    }

    public static float TimeTaken
    {
        get => _timeTaken;
        set => _timeTaken = value;
    }

    public static int PlayerFeedbackDifficulty
    {
        get => _playerFeedbackDifficulty;
        set => _playerFeedbackDifficulty = value;
    }
}
