using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Dino took damage. Health = " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Dino died.");
        Destroy(gameObject); // Or play animation, disable AI, etc.
    }
}
