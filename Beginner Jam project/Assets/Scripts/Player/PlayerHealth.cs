using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float playerHealth = 100;
    private Animator playerAnimator;
    [SerializeField] private Animator armAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    public void playerAttacked(float amount)
    {
        playerHealth -= amount;
        Debug.Log("Health lost: " + amount + ", Health left: " + this.playerHealth);
        if(playerHealth < 0)
        {
            playerHealth = 0;
        }
        if (playerHealth > 100)
        {
            playerHealth = 100;
        }
    }

    public float getPlayerHealth()
    {
        return playerHealth;
    }

    public bool playerDead()
    {
        return playerHealth <= 0;
    }

    public void playerMoving(bool movement)
    {
        
        playerAnimator.SetBool("isWalking", movement);
        armAnimator.SetBool("isWalking", movement);
    }

    public void playerShooting()
    {
        playerAnimator.SetTrigger("Shoot");
        armAnimator.SetTrigger("Shoot");
    }

    public void playerCrouching(bool crouch)
    {
        playerAnimator.SetBool("isCrouching", crouch);
        armAnimator.SetBool("isCrouching", crouch);
    }
}
