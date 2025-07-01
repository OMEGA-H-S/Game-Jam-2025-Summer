using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PterodactylMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Rigidbody2D rb;

    private PterodactylState state;
    private enum PterodactylState
    {
        Idle,
        Attacking,
        Resetting
    }

    private enum Attacks
    {
        Swoop,
        Dive,
        Screech
    }
    
    // Utility Variables
    private float attackTimer;
    bool hitObject;
    bool isSwooping;

    // Changable Values
    private float boostAttackTime = 1f; // Determines how long the pterodactyl boosts for
    
    private void Start()
    {
        attackTimer = boostAttackTime;
        rb = GetComponent<Rigidbody2D>();
        state = PterodactylState.Idle;
        hitObject = false;
        StartCoroutine(Idle());
    }

    private void FixedUpdate()
    {
        float maximumAttackDistance = 15f; // Determines how far the player can be for an attack

        // if the player is within the max attack distance
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < maximumAttackDistance || state != PterodactylState.Idle)

            // TODO Placeholder for which attack the pterodactyl does
            if (false)
            {
                // If the pterodactyl is idle and a 1 in 100 chance hits
                if (Random.Range(0, 100) == 1 && state == PterodactylState.Idle)
                {
                    StopCoroutine(Idle());
                    state = PterodactylState.Attacking;
                }

                // If the pterodactyl is attacking or resetting from one and it's not already swooping, start the swoop coroutine
                if ((state != PterodactylState.Idle) && !isSwooping)
                {
                    isSwooping = true;
                    StartCoroutine(SwoopCoroutine()); // Start Swooping
                }
            } else
            {
                if (Random.Range(0, 100) == 1 && state == PterodactylState.Idle)
                {
                    StopCoroutine(Idle());
                }
            }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        hitObject = true;
    }

    /**
     * Controls the mechanics of the boost attack.
     */
    private void BoostSwoop()
    {

        float pterodactylDesiredYPosition = 2.9f; // Holds the YValue that the pterodactyl floats at
        float yFloatingUpSpeed = 10f; // The Speed that the Pterodactyl resets to the Y position

        // IF the pterodactyl is attacking and it hasn't hit the ground
        if (state == PterodactylState.Attacking && !hitObject)
        {
            Boost(); // Call the boost part of the swoop
        }
        else
        {
            // If its set to be resetting or has hit an object, reset the pterodactyl
            if (state == PterodactylState.Resetting || hitObject)
            {
                ResetPterodactyl(pterodactylDesiredYPosition, yFloatingUpSpeed);
            }
        }
    }

    /**
     * Helper for the BoostSwoop Method. Specifically the boost part of the attack
     */
    private void Boost()
    {
        float boostSpeed = 30f;
        float descendSpeed = 0.1f;

        // Moves the pterodactyl to the correct y position of where the player is
        Vector3 verticalMovementVector = new Vector3(transform.position.x, player.transform.position.y + 0.3f);
        Vector3 newPosition = Vector3.MoveTowards(transform.position, verticalMovementVector, descendSpeed);
        rb.MovePosition(newPosition);
        
        // If the transform has gotten to where it needs to go
        if (transform.position == verticalMovementVector)
        {
            state = PterodactylState.Resetting; // Set the pterodactyl to reset
            
            // Finds the direction the pterodactyl needs to fling towards and "boosts"
            float direction = (player.transform.position - transform.position).x / Mathf.Abs((player.transform.position - transform.position).x);
            rb.velocity = new Vector3(direction * boostSpeed, 0);
        }
    }

    /*
     * Helper for the BoostSwoop method. Specifically resets the pterodactyl.
     */
    private void ResetPterodactyl(float pterodactylDesiredYPosition, float yFloatingUpSpeed)
    {
        if (attackTimer <= 0 || hitObject)
        {
            rb.velocity = Vector3.zero;
            // if the position is less than the desired y value
            if (transform.position.y < pterodactylDesiredYPosition)
            {
                // translate to the y level
                transform.Translate((Vector3.up * yFloatingUpSpeed * Time.deltaTime));
            }
            // otherwise reset all needed variables and such
            else
            {
                rb.velocity = Vector3.zero;
                attackTimer = boostAttackTime;
                state = PterodactylState.Idle;
                isSwooping = false;
                hitObject = false;
                StopCoroutine(SwoopCoroutine());
                StartCoroutine(Idle());
            }
        }
        else
        {
            attackTimer -= Time.deltaTime; // Bring the attack timer down if still attacking
        }
    }

    /**
     * Coroutine methods that makes the pterodactyl move left or right every 2 seconds.
     */
    IEnumerator Idle()
    {
        float idleHorizontalSpeed = 5f; // Controls the horizontal idle speed
        bool isRight = false; // Determines if the pterodactyl is going right or not
        
        while (state == PterodactylState.Idle)
        {
            isRight = !isRight;
            if (isRight)
                rb.velocity = (new Vector2(idleHorizontalSpeed, rb.velocity.y));
            else
                rb.velocity = (new Vector2(-idleHorizontalSpeed, rb.velocity.y));

            yield return new WaitForSeconds(2f);
        }
    }
    
    /**
     * Coroutine methods that controlls the BoostSwoop Attack.
     */
    IEnumerator SwoopCoroutine()
    {
        while (state == PterodactylState.Attacking || state == PterodactylState.Resetting)
        {
            BoostSwoop();
            yield return null;
        }
    }
}
