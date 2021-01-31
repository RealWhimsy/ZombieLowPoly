using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TutorialProgressManager : MonoBehaviour
{
    public GameObject[] bulletZombies;
    public GameObject[] grenadeZombies;
    public GameObject[] meleeZombies;
    public GameObject[] movingZombies;

    public GameObject bulletZombieProgressionWall;
    public GameObject grenadeZombieProgressionWall;
    public GameObject meleeZombieProgressionWall;

    private List<ZombieStatManager> bulletZombiesStatManager = new List<ZombieStatManager>();
    private List<ZombieStatManager> grenadeZombiesStatManager = new List<ZombieStatManager>();
    private List<ZombieStatManager> meleeZombiesStatManager = new List<ZombieStatManager>();
    private List<ZombieStatManager> movingZombiesStatManager = new List<ZombieStatManager>();

    private List<ZombiePathfinding> bulletZombiesPathfinding = new List<ZombiePathfinding>();
    private List<ZombiePathfinding> grenadeZombiesPathfinding = new List<ZombiePathfinding>();
    private List<ZombiePathfinding> meleeZombiesPathfinding = new List<ZombiePathfinding>();
    private List<ZombiePathfinding> movingZombiesPathfinding = new List<ZombiePathfinding>();

    private bool bulletZombiesDead;
    private bool grenadeZombiesDead;
    private bool meleeZombiesDead;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");


    // Start is called before the first frame update
    private void Start()
    {
        EventManager.StartListening(Const.Events.ZombieKilled, CheckForProgressionUnlock);
        AssignCachedVariables();
        MuteZombies();
        StopMovingZombies();
    }

    private void AssignCachedVariables()
    {
        foreach (var zombie in bulletZombies)
        {
            bulletZombiesStatManager.Add(zombie.GetComponent<ZombieStatManager>());
            bulletZombiesPathfinding.Add(zombie.GetComponent<ZombiePathfinding>());
        }
        
        foreach (var zombie in grenadeZombies)
        {
            grenadeZombiesStatManager.Add(zombie.GetComponent<ZombieStatManager>());
            grenadeZombiesPathfinding.Add(zombie.GetComponent<ZombiePathfinding>());
        }
        
        foreach (var zombie in meleeZombies)
        {
            meleeZombiesStatManager.Add(zombie.GetComponent<ZombieStatManager>());
            meleeZombiesPathfinding.Add(zombie.GetComponent<ZombiePathfinding>());
        }
        
        foreach (var zombie in movingZombies)
        {
            movingZombiesStatManager.Add(zombie.GetComponent<ZombieStatManager>());
            movingZombiesPathfinding.Add(zombie.GetComponent<ZombiePathfinding>());
        }
    }

    private void CheckForProgressionUnlock()
    {
        if (!bulletZombiesDead)
        {
            if (CheckIfZombiesAreDead(bulletZombiesStatManager))
            {
                bulletZombiesDead = true;
                UnlockWall(bulletZombieProgressionWall);
            }
        }
        
        else if (!grenadeZombiesDead)
        {
            if (CheckIfZombiesAreDead(grenadeZombiesStatManager))
            {
                grenadeZombiesDead = true;
                UnlockWall(grenadeZombieProgressionWall);
            }
        } 
        
        else if (!meleeZombiesDead)
        {
            if (CheckIfZombiesAreDead(meleeZombiesStatManager))
            {
                meleeZombiesDead = true;
                UnlockWall(meleeZombieProgressionWall);
                EnableMovingZombiesPathfinding();
            }
        }
    }

    private bool CheckIfZombiesAreDead(List<ZombieStatManager> zombies)
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
            Animator animator = zombie.GetComponent<Animator>();

            agent.isStopped = false;
            animator.SetBool(IsMoving, true);
        }
    }

    private void StopMovingZombies()
    {
        NavMeshAgent currentAgent;
        foreach (var zombie in bulletZombiesPathfinding)
        {
            zombie.Target = zombie.gameObject.transform;
        }
        
        foreach (var zombie in grenadeZombiesPathfinding)
        {
            zombie.Target = zombie.gameObject.transform;
        }
        
        foreach (var zombie in meleeZombiesPathfinding)
        {
            zombie.Target = zombie.gameObject.transform;
        }
        
        foreach (var zombie in movingZombies)
        {
            zombie.GetComponent<NavMeshAgent>().isStopped = true;
        }
    }

    // Mutes all Zombies except the moving zombies
    private void MuteZombies()
    {
        foreach (var zombie in bulletZombiesStatManager)
        {
            zombie.Muted = true;
        }
        
        foreach (var zombie in grenadeZombiesStatManager)
        {
            zombie.Muted = true;
        }
        
        foreach (var zombie in meleeZombiesStatManager)
        {
            zombie.Muted = true;
        }
    }
}
