﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieWaveMechanism : MonoBehaviour
{

    public int waves;
    public int zombiesPerWave;
    public float timeBetweenWaves;

    private int waveCounter;
    private int zombieCounter;
    private int killCounterForWave;
    private int tickCounter;

    // Initialize variables and listeners
    void Start()
    {
        zombieCounter = 0;
        waveCounter = 1;
        killCounterForWave = 0;
        EventManager.StartListening(Const.Events.ZombieSpawned, CountZombies);
        EventManager.StartListening(Const.Events.WaveCompleted, ResetVariables);
        EventManager.StartListening(Const.Events.ZombieKilled, CountKills);

    }

    // Resets variables after every wave
    private void ResetVariables()
    {
        if (waveCounter >= waves)
        {
            EventManager.TriggerEvent(Const.Events.StopSpawningZombies);
        }
        zombieCounter = 0;
        killCounterForWave = 0;
        tickCounter = 0;
        waveCounter++;
        Debug.Log(waveCounter + " wave");

    }

    // Counts killed zombies
    private void CountKills()
    {
        killCounterForWave++;

        // Triggers event if every zombie of wave is killed
        if (killCounterForWave >= zombiesPerWave)
        {
            EventManager.TriggerEvent(Const.Events.WavePassed);
            InvokeRepeating("Tick", 0f, 1.0f);
        }
    }

    // Counts spawned zombies
    private void CountZombies()
    {
        zombieCounter++;
        // Triggers event if max number for zombie spawns per wave is reached
        if (zombieCounter >= zombiesPerWave)
        {
            EventManager.TriggerEvent(Const.Events.StopSpawningZombies);          
        }
        
    }

    // Timer between waves
    private void Tick()
    {
        if (tickCounter >= timeBetweenWaves)
        {
            EventManager.TriggerEvent(Const.Events.ResumeSpawningZombies);
            CancelInvoke("Tick");
        }
        Debug.Log("Ticker " + tickCounter);
        tickCounter++;
    }
}
