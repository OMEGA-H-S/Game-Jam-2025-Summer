using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float enemyHealth = 100;

    public void enemyAttacked(float amount)
    {
        enemyHealth -= amount;
        if(enemyHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public float getenemyHealth()
    {
        return enemyHealth;
    }

    public bool enemyDead()
    {
        return enemyHealth <= 0;
    }
}
