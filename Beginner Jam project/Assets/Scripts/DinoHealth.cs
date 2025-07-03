using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoHealth : MonoBehaviour
{
    public int maxHealth = 20;
    private int currentHealth;
    public HealthBar healthBar;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
    }

   
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Dino took damage. Health = " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8) 
        {
            TakeDamage(5); 
            Destroy(other.gameObject);
        }
    }

    void Die()
    {
        Debug.Log("Dino died.");
        Destroy(gameObject); // Or play animation, disable AI, etc.
    }
}
