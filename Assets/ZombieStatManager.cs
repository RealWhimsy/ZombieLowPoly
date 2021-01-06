using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieStatManager : MonoBehaviour, IDamageable, IDamageDealer
{
    public int maxHealth = 100;
    public int armor = 0;
    public int damage = 10;
    public float damageFrequency = 0.3f;
    public System.Random ran = new System.Random();
    //public AudioClip[] hitSoundsArray;
    //private AudioSource audioSource;

    float currentTriggerStayTime;
    Animator anim;
    NavMeshAgent agent;

    InteractiblesManager interactiblesManager;
    bool interactiblesTrigger = false;


    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        currentTriggerStayTime = damageFrequency;
        interactiblesManager = GetComponent<InteractiblesManager>();
        /*audioSource = GetComponent<AudioSource>();
        if(audioSource == null) {
            Debug.LogError("No AudioSource found!");
        }
        LoadSounds();*/
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            anim.SetBool("isDead", true);
            agent.isStopped = true;
            if(interactiblesTrigger == false)
            {
                interactiblesManager.SpawnInteractible();
                interactiblesTrigger = true;
                
            }

        }

      
    }

    /*private void LoadSounds() {
        hitSoundsArray[0] = (AudioClip) Resources.Load("Sounds_Ingame/Hits/hit1");
        hitSoundsArray[1] = (AudioClip) Resources.Load("Sounds_Ingame/Hits/hit2"); 
        hitSoundsArray[2] = (AudioClip) Resources.Load("Sounds_Ingame/Hits/hit3");
        hitSoundsArray[3] = (AudioClip) Resources.Load("Sounds_Ingame/Hits/hit4");
        hitSoundsArray[4] = (AudioClip) Resources.Load("Sounds_Ingame/Hits/dead");
    }*/

    void OnTriggerEnter(Collider collision)
    {
        // Checks if the other GameObject has an IDamageable component. If yes, it deals damage.
        IDamageable damageable = collision.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
        if (damageable != null)
        {
            damageable.TakeDamage(this);
        }

    }

    // Called when a collider stays within the trigger zone
    void OnTriggerStay(Collider other)
    {

        // Checks if the other collider has stayed within the trigger for longer than the damageFreqency
        // If yes, the damage is dealt again. If not, the timer continues to count down
        if (currentTriggerStayTime > 0)
        {
            currentTriggerStayTime -= Time.deltaTime;
        } else
        {
            IDamageable damageable = other.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
            if (damageable != null)
            {
                damageable.TakeDamage(this);
            }     
            currentTriggerStayTime = damageFrequency;
        }
        
    }

    public void TakeDamage(IDamageDealer damageDealer)
    {
        int finalDamage = damageDealer.damage - armor;
        if (finalDamage < 0)
        {
            finalDamage = 0;
        }
        /*int random = ran.Next(0, hitSoundsArray.Length);
        audioSource.clip = hitSoundsArray[random];
        audioSource.PlayOneShot(audioSource.clip);*/
        currentHealth -= finalDamage;
    }

    int IDamageDealer.damage
    {
        get { return damage; }
        set { }
    }
    public DamageType damageType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


}
