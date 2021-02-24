using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Calculates the difficulty changes between waves.
 * Uses the following metrics:
 * 
 * ShotScore: % of shots hit. A hit is counted if it hits a special hitbox on the zombies, which is bigger than the
 *              normal damage hitbox. This means, that closely missed shots also count as a hit for the purpose of
 *              calculating the difficulty. Shots that only hit the "special hitbox" do not deal damage.
 *
 * GrenadeScore: Grenades thrown vs zombies hit by grenades. The player gets a better score when their grenades hit many
 *                  Zombies at once. Grenades that hit few zombies or no zombies at all lead to a worse score.
 *
 * DamageTakenScore: How much damage the player has taken in a single wave. Less damage taken means a better score.
 *
 * TimeScore: The time it took for the player to complete the wave. Less time taken means a better score.
 *
 * PlayerFeedbackScore: Applied once after the player chose an option in the level completed dialogue. Difficulty changes
 *                      based on which button the player pressed.
 */
public class DDAManager : MonoBehaviour
{
    // ---- the fields below are carried over between levels and rounds ----
    // ---- this is done to prevent radical difficulty changes if the player has a lucky/unlucky round ----
    private static int _shotsFired;
    private static int _shotsHit;
    private static int _grenadesThrown;
    private static int _grenadesHit;
    // ---- end of carried over fields ---

    // ---- the fields below are reset after each wave ----
    private static int _damageTaken;
    private static DateTime startTime;
    private static DateTime endTime;
    // ---- end of fields that are reset after each wave ----
    
    // ---- the fields below are reset for each completed level, but not between waves ----
    private static int _zombiesKilled;
    private static int _playerDeaths;
    // ---- end of level specific stats ----
    
    private static int _playerFeedbackDifficulty; //chosen by the player in the "Level Completed" view via button press


    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(Const.Events.ShotFired, () => _shotsFired++);
        EventManager.StartListening(Const.Events.ShotHitDDAZone, () => _shotsHit++);
        EventManager.StartListening(Const.Events.GrenadeThrown, () => _grenadesThrown++);
        EventManager.StartListening(Const.Events.GrenadeHit, () => _grenadesHit++);
        EventManager.StartListening(Const.Events.ZombieKilled, () => _zombiesKilled++);
        EventManager.StartListening(Const.Events.PlayerDead, HandlePlayerDeath);

        EventManager.StartListening(Const.Events.WaveCompleted, AdjustDDA);
        EventManager.StartListening(Const.Events.DifficultyChanged, HandleDifficultyChange);
        EventManager.StartListening(Const.Events.WaveStarted, WaveStarted);
        EventManager.StartListening(Const.Events.TutorialCompleted, AdjustDDA);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        startTime = DateTime.Now;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetLevelSpecificStats();
    }

    private void ResetLevelSpecificStats()
    {
        _playerDeaths = 0;
        _zombiesKilled = 0;
    }

    private void AdjustDDA()
    {
        var difficultyChange = CalculateTotalDifficultyChange();

        Difficulty.CurrentDifficultyIndex += difficultyChange;
        Difficulty.ApplyDifficultyChange();
    }

    private int CalculateTotalDifficultyChange()
    {
        var shotScore = CalculateShotScore();
        var grenadeScore = CalculateGrenadeScore();
        var damageTakenScore = CalculateDamageTakenScore();
        var timeScore = CalculateTimeScore();
        var difficultyChange = shotScore + grenadeScore + damageTakenScore + timeScore + _playerFeedbackDifficulty;

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

    private void HandlePlayerDeath()
    {
        _playerDeaths++;

        // every other death lower difficulty by one
        if (_playerDeaths % 2 == 0)
        {
            Difficulty.CurrentDifficultyIndex--;
            Difficulty.ApplyDifficultyChange();
        }

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
        _playerFeedbackDifficulty = 0; // only apply player feedback difficulty once, after the level completed dialogue
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

    public static int PlayerFeedbackDifficulty
    {
        get => _playerFeedbackDifficulty;
        set => _playerFeedbackDifficulty = value;
    }

    public static int ZombiesKilled
    {
        get => _zombiesKilled;
        set => _zombiesKilled = value;
    }

    public static int PlayerDeaths
    {
        get => _playerDeaths;
        set => _playerDeaths = value;
    }
}