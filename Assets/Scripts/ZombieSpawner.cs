using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    private GameObject[] zombieSpawners;
    public GameObject[] zombiePrefabs;

    private System.Random random = new System.Random();
    private PlayerManager playerManager;
    private GameObject player;

    public float initialSpawnDelay = 0f;
    public float timeBetweenRespawns = 10f;

    // Minimum and maximum distances between player and spawnpoint. 
    // Zombies will not spawn if player is too close or too far
    public float minDistance;
    public float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        zombieSpawners = GameObject.FindGameObjectsWithTag("ZombieSpawn");

        InvokeRepeating("SpawnZombie", initialSpawnDelay, timeBetweenRespawns);

        player = GameObject.FindGameObjectWithTag("Player");

        playerManager = player.GetComponent<PlayerManager>();

        EventManager.StartListening(Const.Events.StopSpawningZombies, StopSpawningZombies);
        EventManager.StartListening(Const.Events.ResumeSpawningZombies, ResumeSpawningZombies);
    }

    void Update()
    {
        // Stop spawning zombies if the player is dead
        if (playerManager.isDead())
        {
            CancelInvoke();
        }
    }

    void SpawnZombie()
    {
        foreach (GameObject spawner in zombieSpawners)
        {
            // get a random zombie out of all the prefabs
            int index = random.Next(zombiePrefabs.Length);

            float distance = Vector3.Distance(player.transform.position, spawner.transform.position);

            if (distance >= minDistance && distance <= maxDistance)
            {
                GameObject spawnedZombie = Instantiate(zombiePrefabs[index], spawner.transform);
                EventManager.TriggerEvent(Const.Events.ZombieSpawned);
                /* 
                    * For some reason the zombies would be placed randomly around the spawner with Instantiate
                    * The two lines below warp the zombie to where it is supposed to be after spawning
                    */
                NavMeshAgent agent = spawnedZombie.GetComponent<NavMeshAgent>();
                agent.Warp(spawner.transform.position);
            }
        }
        
    }

    // Stops spawning zombies
    private void StopSpawningZombies()
    {
        CancelInvoke("SpawnZombie");
    }

    // Resumes spawning zombies
    private void ResumeSpawningZombies()
    {
        InvokeRepeating("SpawnZombie", initialSpawnDelay, timeBetweenRespawns);     
    }
}
