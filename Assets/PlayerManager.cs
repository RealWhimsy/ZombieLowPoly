using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageable
{

    public int maxHealth = 200;
    public int armor = 5;
    public HealthBar healthBar;

    int currentHealth;
    bool dead = false;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            dead = true;
            anim.SetBool("isDead", true);
        }
    }

    public void TakeDamage(IDamageDealer damageDealer)
    {
        int finalDamage = damageDealer.damage - armor;
        if (finalDamage < 0)
        {
            finalDamage = 0;
        }

        currentHealth -= finalDamage;
        healthBar.SetHealth(currentHealth);

        Debug.Log("Player took " + finalDamage + " damage. Current Health: " + currentHealth);
    }

    public bool isDead()
    {
        return dead;
    }
}
