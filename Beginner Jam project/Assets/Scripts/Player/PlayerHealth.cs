using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private float playerHealth = 100;
    private Animator playerAnimator;
    [SerializeField] private Animator armAnimator;
    [SerializeField] private HealthBar bar;
    [SerializeField] private AudioClip damage;
    [SerializeField] private AudioClip death;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        bar.SetMaxHealth((int)playerHealth);
    }

    public void playerAttacked(float amount)
    {
        playerHealth -= amount;
        
        if(playerHealth < 0)
        {
            SoundEffectsManager.instance.PlaySoundEffectClip(death, transform, 1f);
            playerHealth = 0;
            SceneManager.LoadScene("Home");

        }
        else if (amount > 0)
        {
            SoundEffectsManager.instance.PlaySoundEffectClip(damage, transform, 1f);
        }
        if (playerHealth > 100)
        {
            playerHealth = 100;
        }
        
        bar.SetHealth((int)playerHealth);
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
