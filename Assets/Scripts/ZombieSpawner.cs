using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ZombieSpawner : MonoBehaviour
{
    private GameObject[] zombieSpawners;
    public GameObject[] zombiePrefabs;

    private System.Random random = new System.Random();

    public float initialSpawnDelay = 0f;
    public float timeBetweenRespawns = 10f;
    public GameObject zombie;

    PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        zombieSpawners = GameObject.FindGameObjectsWithTag("ZombieSpawn");

        InvokeRepeating("SpawnZombie", initialSpawnDelay, timeBetweenRespawns);

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
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

            GameObject spawnedZombie = Instantiate(zombiePrefabs[index], spawner.transform);

            //spawnedZombie.transform.position = spawner.transform.position;
        }
    }
}
