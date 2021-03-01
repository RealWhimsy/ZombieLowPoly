using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoggingManager : MonoBehaviour
{
    private const string FilePath = "./logs/";
    private const string WaveCompletedMessage = "WaveCompleted";
    private const string LevelCompletedMessage = "LevelCompleted";
    private const string PlayerDiedMessage = "PlayerDied";
    private static string _playerId = Guid.NewGuid().ToString();
    private StreamWriter writer;

    private int waveNumber = 1;
    private string waveTag;
    private string levelTag;

    private DateTime waveStartTime;
    private DateTime waveEndTime;
    private DateTime levelStartTime;
    private DateTime levelEndTime;

    private string currentSceneName = "";

    // ---- data ----
    private int shotsInCurrentWave;
    private int hitsInCurrentWave;
    private static int _damageTakenInCurrentWave;
    private int grenadesThrownInCurrentWave;
    private int grenadesHitInCurrentWave;
    private int timeSpentInCurrentWave;
    private int deathsInCurrentWave;
    private int interactiblesCollectedInCurrentWave;
    private int meleeAttacksInCurrentWave;
    private int waveDifficultyIndex;
    private int triesForThisLevel = 1;

    private static int _shotsTotal;
    private static int _hitsTotal;
    private static int _damageTakenTotal;
    private static int _grenadesThrownTotal;
    private static int _grenadesHitTotal;
    private static int _timeSpentTotal;
    private static int _deathsTotal;
    private static int _interactiblesCollectedTotal;
    private static int _meleeAttacksTotal;

    private static string _isFirstLogCall = "true";

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        EventManager.StartListening(Const.Events.ShotFired, () =>
        {
            shotsInCurrentWave++;
            _shotsTotal++;
        });

        EventManager.StartListening(Const.Events.ShotHitDDAZone, () =>
        {
            hitsInCurrentWave++;
            _hitsTotal++;
        });

        EventManager.StartListening(Const.Events.GrenadeThrown, () =>
        {
            grenadesThrownInCurrentWave++;
            _grenadesThrownTotal++;
        });


        EventManager.StartListening(Const.Events.GrenadeHit, () =>
        {
            grenadesHitInCurrentWave++;
            _grenadesHitTotal++;
        });

        EventManager.StartListening(Const.Events.PlayerDead, () =>
        {
            deathsInCurrentWave++;
            _deathsTotal++;
            waveNumber = 1;
            triesForThisLevel++;
            LogWaveStats(PlayerDiedMessage);
            ResetPerWaveStats();
        });

        EventManager.StartListening(Const.Events.InteractibleCollected, () =>
        {
            interactiblesCollectedInCurrentWave++;
            _interactiblesCollectedTotal++;
        });

        EventManager.StartListening(Const.Events.MeleeAttack, () =>
        {
            meleeAttacksInCurrentWave++;
            _meleeAttacksTotal++;
        });

        EventManager.StartListening(Const.Events.WaveCompleted, HandleWaveCompleted);
        EventManager.StartListening(Const.Events.LevelCompleted, HandleLevelCompleted);
        EventManager.StartListening(Const.Events.WaveStarted, HandleWaveStarted);
        EventManager.StartListening(Const.Events.DifficultySelected, LogDifficultyFeedback);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // set start time of the new level
        // if-statement is needed so stats don't reset if the player dies and respawns (same scene is loaded again)
        SetWaveTag();
        if (!scene.name.Equals(currentSceneName))
        {
            waveNumber = 1;
            levelStartTime = DateTime.Now;
            currentSceneName = scene.name;
        }
    }

    private void WriteCsvHeader()
    {
        writer.WriteLine(LogEntry.HeaderRow() + "\n");
        writer.Flush();
    }

    private void HandleWaveCompleted()
    {
        SetWaveTag();
        waveNumber++;
        LogWaveStats(WaveCompletedMessage);
        ResetPerWaveStats();
    }

    private void HandleWaveStarted()
    {
        waveStartTime = DateTime.Now;
        waveDifficultyIndex = Difficulty.CurrentDifficultyIndex;
    }

    private void SetWaveTag()
    {
        waveTag = SceneManager.GetActiveScene().name + "Wave" + waveNumber;
    }

    private void HandleLevelCompleted()
    {
        SetLevelTag();
        LogTotalStats();

        // reset the stats from the last wave of the level as well
        ResetPerWaveStats();
        triesForThisLevel = 1;
    }

    private void SetLevelTag()
    {
        levelTag = SceneManager.GetActiveScene().name + "Completed";
    }

    private void LogTotalStats()
    {
        levelEndTime = DateTime.Now;
        TimeSpan timeSpan = levelEndTime - levelStartTime;
        double seconds = timeSpan.TotalSeconds;
        seconds = Math.Round(seconds);
        
        WWWForm form = new WWWForm();
        form.AddField(Const.PhpVariables.IsFirstLogCall, _isFirstLogCall);
        form.AddField(Const.PhpVariables.Id, _playerId);
        form.AddField(Const.PhpVariables.Message, LevelCompletedMessage);
        form.AddField(Const.PhpVariables.Tag, levelTag);
        form.AddField(Const.PhpVariables.ShotsFired, _shotsTotal);
        form.AddField(Const.PhpVariables.ShotsHit, _hitsTotal);
        form.AddField(Const.PhpVariables.DamageTaken, _damageTakenTotal);
        form.AddField(Const.PhpVariables.GrenadesThrown, _grenadesThrownTotal);
        form.AddField(Const.PhpVariables.GrenadesHit, _grenadesHitTotal);
        form.AddField(Const.PhpVariables.TimeSpent, (int) seconds);
        form.AddField(Const.PhpVariables.DeathCount, _deathsTotal);
        form.AddField(Const.PhpVariables.InteractiblesCollected, _interactiblesCollectedTotal);
        form.AddField(Const.PhpVariables.MeleeAttacks, _meleeAttacksTotal);
        form.AddField(Const.PhpVariables.DifficultyIndex, waveDifficultyIndex);
        form.AddField(Const.PhpVariables.TriesForLevel, triesForThisLevel);

        StartCoroutine(logDataToFile(form));
    }

    private void LogWaveStats(String message)
    {
        waveEndTime = DateTime.Now;
        TimeSpan timeSpan = waveEndTime - waveStartTime;
        double seconds = timeSpan.TotalSeconds;
        seconds = Math.Round(seconds); // round to the nearest integer for easier to read data

        WWWForm form = new WWWForm();
        form.AddField(Const.PhpVariables.IsFirstLogCall, _isFirstLogCall);
        form.AddField(Const.PhpVariables.Id, _playerId);
        form.AddField(Const.PhpVariables.Message, message);
        form.AddField(Const.PhpVariables.Tag, waveTag);
        form.AddField(Const.PhpVariables.ShotsFired, shotsInCurrentWave);
        form.AddField(Const.PhpVariables.ShotsHit, hitsInCurrentWave);
        form.AddField(Const.PhpVariables.DamageTaken, _damageTakenInCurrentWave);
        form.AddField(Const.PhpVariables.GrenadesThrown, grenadesThrownInCurrentWave);
        form.AddField(Const.PhpVariables.GrenadesHit, grenadesHitInCurrentWave);
        form.AddField(Const.PhpVariables.TimeSpent, (int) seconds);
        form.AddField(Const.PhpVariables.DeathCount, deathsInCurrentWave);
        form.AddField(Const.PhpVariables.InteractiblesCollected, interactiblesCollectedInCurrentWave);
        form.AddField(Const.PhpVariables.MeleeAttacks, meleeAttacksInCurrentWave);
        form.AddField(Const.PhpVariables.DifficultyIndex, waveDifficultyIndex);
        form.AddField(Const.PhpVariables.TriesForLevel, triesForThisLevel);

        StartCoroutine(logDataToFile(form));
    }

    private void LogDifficultyFeedback()
    {
        WWWForm form = new WWWForm();
        form.AddField(Const.PhpVariables.Id, _playerId);
        form.AddField(Const.PhpVariables.Tag, levelTag);
        form.AddField(Const.PhpVariables.SelectedDifficulty, LevelCompletedMenu.SelectedDifficulty);

        StartCoroutine(LogDifficultyFeedbackToFile(form));
    }

    private static IEnumerator LogDifficultyFeedbackToFile(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(Const.ServerURL + "php/difficulty-logger.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload successful!");
            }
        }
    }

    IEnumerator logDataToFile(WWWForm form)
    {
        _isFirstLogCall = "false";

        using (UnityWebRequest www = UnityWebRequest.Post(Const.ServerURL + "php/webgl-logger.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload successful!");
            }
        }
    }


    private void ResetPerWaveStats()
    {
        shotsInCurrentWave = 0;
        hitsInCurrentWave = 0;
        grenadesThrownInCurrentWave = 0;
        grenadesHitInCurrentWave = 0;
        _damageTakenInCurrentWave = 0;
        deathsInCurrentWave = 0;
        interactiblesCollectedInCurrentWave = 0;
    }

    public static int DamageTakenInCurrentWave
    {
        get => _damageTakenInCurrentWave;
        set => _damageTakenInCurrentWave = value;
    }

    public static int DamageTakenTotal
    {
        get => _damageTakenTotal;
        set => _damageTakenTotal = value;
    }
    
    public static string PlayerId => _playerId;
}