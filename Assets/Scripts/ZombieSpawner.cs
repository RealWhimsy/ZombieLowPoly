using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    GameObject[] zombieSpawners;

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
            GameObject spawnedZombie = Instantiate(zombie);

            spawnedZombie.transform.position = spawner.transform.position;
        }
    }
}
