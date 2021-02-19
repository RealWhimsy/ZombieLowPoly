﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePathfinding : MonoBehaviour
{

    public Transform target;

    Animator anim;
    NavMeshAgent agent;
    PlayerManager playerManager;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        EventManager.StartListening(Const.Events.PlayerRespawned, HandlePlayerRespawn);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();

        // set player as the target if no target is set in inspector
        if (target == null)
        {
            target = player.transform;
        }

        SetCurrentDifficultyStats();
    }

    private void SetCurrentDifficultyStats()
    {
        agent.speed = Difficulty.CurrentDifficulty.ZombieSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Chase the player until he is dead
        if (!playerManager.isDead())
        {
            agent.SetDestination(target.position);
            anim.SetBool("isMoving", !checkIfTargetReached());
        }
        else
        {
            anim.SetBool("isMoving", false);
            agent.isStopped = true;
        }
    }

    // Checks if the Object has finished navigating towards the target
    // returns false if navigation is finished
    bool checkIfTargetReached()
    {
        if (Vector3.Distance (agent.gameObject.transform.position, target.position) <= agent.stoppingDistance)
        {
            return true;
        }
        
        return false;
    }

    private void HandlePlayerRespawn()
    {
        // Re-enable pathfinding after player respawned
        agent.isStopped = false;
    }

    public Transform Target
    {
        get => target;
        set => target = value;
    }
}
