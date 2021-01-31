using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TutorialProgressManager : MonoBehaviour
{
    public ZombieStatManager[] bulletZombies;
    public ZombieStatManager[] grenadeZombies;
    public ZombieStatManager[] meleeZombies;
    public GameObject[] movingZombies;

    public GameObject bulletZombieProgressionWall;
    public GameObject grenadeZombieProgressionWall;
    public GameObject meleeZombieProgressionWall;

    private bool bulletZombiesDead;
    private bool grenadeZombiesDead;
    private bool meleeZombiesDead;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        EventManager.StartListening(Const.Events.ZombieKilled, CheckForProgressionUnlock);
    }

    private void CheckForProgressionUnlock()
    {
        if (!bulletZombiesDead)
        {
            if (CheckIfZombiesAreDead(bulletZombies))
            {
                bulletZombiesDead = true;
                UnlockWall(bulletZombieProgressionWall);
            }
        }
        
        else if (!grenadeZombiesDead)
        {
            if (CheckIfZombiesAreDead(grenadeZombies))
            {
                grenadeZombiesDead = true;
                UnlockWall(grenadeZombieProgressionWall);
            }
        } 
        
        else if (!meleeZombiesDead)
        {
            if (CheckIfZombiesAreDead(meleeZombies))
            {
                meleeZombiesDead = true;
                UnlockWall(meleeZombieProgressionWall);
                EnableMovingZombiesPathfinding();
            }
        }
    }

    private bool CheckIfZombiesAreDead(ZombieStatManager[] zombies)
    {
        foreach (var zombie in zombies)
        {
            if (!zombie.IsDead)
            {
                return false;
            }
        }

        return true;
    }

    private void UnlockWall(GameObject wall)
    {
        wall.SetActive(false);
    }

    private void EnableMovingZombiesPathfinding()
    {
        foreach (var zombie in movingZombies)
        {
            NavMeshAgent agent = zombie.GetComponent<NavMeshAgent>();

            if (agent != null)
            {
                agent.enabled = true;
            }
        }
    }
}
