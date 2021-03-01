using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class TutorialProgressManager : MonoBehaviour
{
    public GameObject[] bulletZombies;
    public GameObject[] grenadeZombies;
    public GameObject[] meleeZombies;
    public GameObject[] movingZombies;

    public GameObject[] grenadeInteractibles;
    private Vector3[] grenadeInteractiblePositions;
    

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
    private bool movingZombiesDead;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");


    // Start is called before the first frame update
    private void Start()
    {
        EventManager.StartListening(Const.Events.ZombieKilled, CheckForProgressionUnlock);
        EventManager.StartListening(Const.Events.InteractibleCollected, StartRefillGrenadeCoroutine);
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

        grenadeInteractiblePositions = new Vector3[3];
        grenadeInteractiblePositions[0] = Const.Tutorial.GrenadeBoxOneVector;
        grenadeInteractiblePositions[1] = Const.Tutorial.GrenadeBoxTwoVector;
        grenadeInteractiblePositions[2] = Const.Tutorial.GrenadeBoxThreeVector;
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

        else if (!movingZombiesDead)
        {
            if (CheckIfZombiesAreDead(movingZombiesStatManager))
            {
                movingZombiesDead = true;
                EventManager.TriggerEvent(Const.Events.LevelCompleted);
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

    private void StartRefillGrenadeCoroutine()
    {
        StartCoroutine(InstantiateGrenadeInteractible());
    }

    private IEnumerator InstantiateGrenadeInteractible()
    {
        yield return new WaitForSeconds(5);
        for (int i = 0; i < grenadeInteractibles.Length; i++)
        {
            if (!grenadeInteractibles[i])
            {
                grenadeInteractibles[i] = InteractiblesManager.SpawnGuaranteedInteractible(
                    grenadeInteractiblePositions[i], new Quaternion(0f, 0f, 0f, 0f));
            }
        }
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