using System.Collections;
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
    private bool wavePassedBool = false;
    private bool noZombieGameObject = false;

    // Initialize variables and listeners
    void Start()
    {
        zombieCounter = 0;
        waveCounter = 1;
        killCounterForWave = 0;
        EventManager.StartListening(Const.Events.ZombieSpawned, CountZombies);
        EventManager.StartListening(Const.Events.WaveCompleted, ResetVariables);
        EventManager.StartListening(Const.Events.ZombieKilled, CountKills);
        EventManager.StartListening(Const.Events.ZombieKilled, GetZombieGameObjects);

    }

    void Update()
    {
        if((noZombieGameObject == true) && (killCounterForWave >= zombiesPerWave) && (wavePassedBool == false))
        {
            WavePassed();
            wavePassedBool = true;
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
        zombieCounter = 0;
        killCounterForWave = 0;
        waveCounter++;
        wavePassedBool = false;

    }

    private void GetZombieGameObjects()
    {
        Invoke("GetGameObjects", 2);
    }

    private void GetGameObjects()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            noZombieGameObject = true;
        }
    }

    // Counts killed zombies
    private void CountKills()
    {
        noZombieGameObject = false;
        killCounterForWave++;
    }

    // Triggers event if wave is passed
    private void WavePassed()
    {
        EventManager.TriggerEvent(Const.Events.WavePassed);
        StartCoroutine(Tick());
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

    // Timer between waves - resets all variables
    private IEnumerator Tick()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        EventManager.TriggerEvent(Const.Events.ResumeSpawningZombies);
        ResetVariables();
    }
}
