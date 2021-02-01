using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ZombieStatManager : MonoBehaviour, IDamageable, IDamageDealer
{
    public int maxHealth = 100;
    public int armor = 0;
    public int damage = 10;
    public float damageFrequency = 0.3f;
    public System.Random ran = new System.Random();
    public DamageType[] damagableByTypes;
    
    private GameObject blood;
    private AudioClip[] hitMarker;
    private AudioClip[] zombieSounds;
    private AudioSource zombieAudio;
    private bool muted;

    float currentTriggerStayTime;
    Animator anim;
    NavMeshAgent agent;

    InteractiblesManager interactiblesManager;
    bool interactiblesTrigger;
    bool isDead;
    bool playSound;


    int currentHealth;

    private static readonly int IsDeadAnimation = Animator.StringToHash("isDead");

    // Start is called before the first frame update
    void Start()
    {
        blood = Resources.Load("Prefabs/Blood") as GameObject;

        zombieAudio = GetComponent<AudioSource>();
        hitMarker = Resources.LoadAll<AudioClip>(Const.SFX.Hits);
        zombieSounds = Resources.LoadAll<AudioClip>(Const.SFX.Zombies);

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        currentTriggerStayTime = damageFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            anim.SetBool(IsDeadAnimation, true);
            agent.isStopped = true;
            zombieAudio.Stop();
            Destroy(gameObject.GetComponent<BoxCollider>());

            // Trigger for spawning interactibles only one time
            if (interactiblesTrigger == false)
            {
                InteractiblesManager.SpawnInteractible(transform.position, transform.rotation);
                interactiblesTrigger = true;
            }

            // Trigger for counting kills
            if(isDead == false)
            {
                isDead = true;
                EventManager.TriggerEvent(Const.Events.ZombieKilled);
            }

        }

        if(playSound == false && !muted)
        {
            playSound = true;
            StartCoroutine(PlayIdleSounds());
        }

      
    }

    IEnumerator PlayIdleSounds()
    {
        float time = (float) GameAssets.i.GenerateRandomNumber(0, 6);
        yield return new WaitForSeconds(time);
        zombieAudio.clip = zombieSounds[GameAssets.i.GenerateRandomNumber(0, zombieSounds.Length-1)];
        zombieAudio.PlayOneShot(zombieAudio.clip);
        playSound = false;
    }

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
        // Do not take damage if source is from another enemy (prevent friendly fire)
        if (damageDealer.damageSource == DamageSource.Enemy)
        {
            return;
        }

        if (damagableByTypes.Length >= 1 && !damagableByTypes.Contains(damageDealer.damageType))
        {
            return;
        }
        
        int finalDamage = damageDealer.damage - armor;
        if (finalDamage < 0)
        {
            finalDamage = 0;
        }
        SoundManagerRework.Instance.RandomSoundEffect(hitMarker);
        Instantiate(blood, transform.position, transform.rotation);
        currentHealth -= finalDamage;
    }

    int IDamageDealer.damage
    {
        get { return damage; }
        set { }
    }
    public DamageType damageType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public DamageSource damageSource
    {
        get => DamageSource.Enemy;
        set {}
    }

    public bool IsDead => isDead;

    public bool Muted
    {
        get => muted;
        set => muted = value;
    }
}
