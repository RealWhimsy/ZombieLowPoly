using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class AbstractDamageableEntity : MonoBehaviour
{
    int currentHealth;
    int armor;
    void TakeDamage(int damage)
    {
        int finalDamage = damage - armor;
        if (finalDamage < 0)
        {
            finalDamage = 0;
        }

        currentHealth -= finalDamage;
    }
}