using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoggingManager : MonoBehaviour
{
    private const string FilePath = "./logs/";
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

    private static int _shotsTotal;
    private static int _hitsTotal;
    private static int _damageTakenTotal;
    private static int _grenadesThrownTotal;
    private static int _grenadesHitTotal;
    private static int _timeSpentTotal;
    private static int _deathsTotal;
    private static int _interactiblesCollectedTotal;
    private static int _meleeAttacksTotal;

    void Start()
    {
        string fileName = Guid.NewGuid() + ".csv";
        writer = new StreamWriter(FilePath + fileName);
        WriteCsvHeader();

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
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // set start time of the new level
        // if-statement is needed so stats don't reset if the player dies and respawns (same scene is loaded again)
        if (!scene.name.Equals(currentSceneName))
        {
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
        LogWaveStats();
        ResetPerWaveStats();
    }

    private void HandleWaveStarted()
    {
        waveStartTime = DateTime.Now;
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

        LogEntry entry = new LogEntry(levelTag, _shotsTotal, _hitsTotal, _damageTakenTotal, _grenadesThrownTotal,
            _grenadesHitTotal, seconds, _deathsTotal, _interactiblesCollectedTotal, _meleeAttacksTotal);

        writer.WriteLine(entry + "\n");
        writer.Flush();
    }

    private void LogWaveStats()
    {
        waveEndTime = DateTime.Now;
        TimeSpan timeSpan = waveEndTime - waveStartTime;
        double seconds = timeSpan.TotalSeconds;
        seconds = Math.Round(seconds); // round to the nearest integer for easier to read data

        LogEntry entry = new LogEntry(waveTag, shotsInCurrentWave, hitsInCurrentWave, _damageTakenInCurrentWave,
            grenadesThrownInCurrentWave,
            grenadesHitInCurrentWave, seconds, deathsInCurrentWave, interactiblesCollectedInCurrentWave,
            meleeAttacksInCurrentWave);

        writer.WriteLine(entry + "\n");
        writer.Flush();
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
}