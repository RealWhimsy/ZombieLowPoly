using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieWaveMechanism : MonoBehaviour
{

    public int waves;
    public int zombiesPerWave;
    public float timeBetweenWaves;

    private int waveCounter;
    private int aliveZombiesCounter;
    private int totalSpawnedZombiesCounter;
    private int killCounterForWave;
    private bool wavePassed;

    // Initialize variables and listeners
    void Start()
    {
        aliveZombiesCounter = 0;
        waveCounter = 1;
        killCounterForWave = 0;
        EventManager.StartListening(Const.Events.ZombieSpawned, CountZombies);
        EventManager.StartListening(Const.Events.ZombieKilled, CountKills);
    }

    void Update()
    {
        if(aliveZombiesCounter <= 0 && killCounterForWave >= zombiesPerWave && !wavePassed)
        {
            WavePassed();
            wavePassed = true;
        }
    }

    // Resets variables after every wave
    private void ResetVariables()
    {
        if (waveCounter >= waves)
        {
            EventManager.TriggerEvent(Const.Events.StopSpawningZombies);
            EventManager.TriggerEvent(Const.Events.LevelCompleted);
        }
        aliveZombiesCounter = 0;
        killCounterForWave = 0;
        totalSpawnedZombiesCounter = 0;
        waveCounter++;
        wavePassed = false;
    }

    // Counts killed zombies
    private void CountKills()
    {
        killCounterForWave++;
        aliveZombiesCounter--;
    }

    // Triggers event if wave is passed
    private void WavePassed()
    {
        EventManager.TriggerEvent(Const.Events.WaveCompleted);
        StartCoroutine(Tick());
    }

    // Counts spawned zombies
    private void CountZombies()
    {
        aliveZombiesCounter++;
        totalSpawnedZombiesCounter++;
        // Triggers event if max number for zombie spawns per wave is reached
        if (totalSpawnedZombiesCounter >= zombiesPerWave)
        {
            EventManager.TriggerEvent(Const.Events.StopSpawningZombies);          
        }
        
    }

    // Timer between waves - resets all variables
    private IEnumerator Tick()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        EventManager.TriggerEvent(Const.Events.ResumeSpawningZombies);
        ResetVariables();
    }

    public int TotalSpawnedZombiesCounter => totalSpawnedZombiesCounter;
}
