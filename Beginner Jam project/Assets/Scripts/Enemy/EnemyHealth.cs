using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private SecurityMovement enemy;
    private float enemyHealth = 100;
    private Animator anim;
    private string color = "Red";
    [SerializeField] private GameObject gunPivot;
    private HealthBar bar;

    private void Start()
    {
        enemy = GetComponent<SecurityMovement>();
        if(enemy.getDifficulty() == SecurityMovement.Difficulty.Easy)
        {
            color = "Green";
            enemyHealth = 25;
        }
        else if(enemy.getDifficulty() == SecurityMovement.Difficulty.Medium)
        {
            color = "Blue";
            enemyHealth = 40;
        }
        else if(enemy.getDifficulty() == SecurityMovement.Difficulty.Hard)
        {
            color = "Red";
            enemyHealth = 50;
        }
        else
        {
            color = "Boss";
            enemyHealth = 100;
        }

        anim = GetComponent<Animator>();
        bar = GetComponentInChildren<HealthBar>();
        bar.SetMaxHealth((int)enemyHealth);
    }

    public void enemyAttacked(float amount)
    {
        enemyHealth -= amount;
        bar.SetHealth((int)enemyHealth);
        if(enemyHealth <= 0)
        {
            gunPivot.SetActive(false);
            GetComponent <BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
            anim.SetTrigger(color);
            anim.SetBool("isDead", true);
        }
    }

    public float getenemyHealth()
    {
        return enemyHealth;
    }

    public void enemyDead()
    {

        Destroy(this.gameObject);

    }




}
