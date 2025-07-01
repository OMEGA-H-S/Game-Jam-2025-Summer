using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PterodactylMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject spriteObject;
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
    bool isDiving;

    bool isScreeching;
    bool isAtVector;

    // Changable Values
    private float boostAttackTime = 1f; // Determines how long the pterodactyl boosts for
    float pterodactylDesiredYPosition = 2.9f; // Holds the YValue that the pterodactyl floats at
    float yFloatingUpSpeed = 10f; // The Speed that the Pterodactyl resets to the Y position
    float screechDistance = 15f; // Distance the pterodactyl keeps from player while screeching


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
            } else if (false) 
            {

                if (Random.Range(0, 100) == 1 && state == PterodactylState.Idle)
                {
                    attackTimer = 5f;
                    state = PterodactylState.Attacking;
                    StartCoroutine(DiveCoroutiune());
                }
            } else
            {
                if (Random.Range(0, 100) == 1 && state == PterodactylState.Idle)
                {
                    attackTimer = 5f;
                    state = PterodactylState.Attacking;
                    StartCoroutine(ScreechCoroutine());
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
                if (attackTimer <= 0 || hitObject)
                {
                    ResetPterodactyl(pterodactylDesiredYPosition, yFloatingUpSpeed);
                }
                else
                {
                    attackTimer -= Time.deltaTime; // Bring the attack timer down if still attacking
                }
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

    /**
     * Deals with Mechancs for the Dive attack.
     */
    private void Dive()
    {
        float aboveYValue = 17f; // Value the pterodactyl needs to get up to before diving

        // if attacking
        if (state == PterodactylState.Attacking)
        {

            // Move to the y position over the player
            Vector3 desiredDiveVector3 = new Vector3(player.transform.position.x, aboveYValue);
            Vector3 diveVector = Vector3.MoveTowards(transform.position, desiredDiveVector3, 1f);
            if (!isDiving) {
                rb.MovePosition(diveVector);
            }

            // If we've reached the hight and we aren't diving yet
            if (transform.position == desiredDiveVector3 && !isDiving)
            {
                // stop the movement, and delay the dive
                rb.velocity = Vector3.zero;
                isDiving = true;
                StartCoroutine(DiveDelay()); // Starts the dive
            }
        } else
        {
            // If the attack timer is done, reset the pteroactyl
            if (attackTimer <= 0) {
                ResetPterodactyl(pterodactylDesiredYPosition, yFloatingUpSpeed);
            } else
            {
                attackTimer -= Time.fixedDeltaTime;
            }
        }
    }

    /**
     * Deals with the mechanics of the screech attack.
     */
    private void ScreechAttack()
    {
        
        if (state == PterodactylState.Attacking)
        {
            Screech(); // Attack Portion
        }
        else
        {
            ResetPterodactyl(pterodactylDesiredYPosition, yFloatingUpSpeed); // Reset Portion
        }
    }

    /**
     * Helper for ScreechAttack().
     * 
     * Specifically deals with the attack portion.
     */
    private void Screech()
    {
        float descentSpeed = 0.4f;

        // Make the Pterodactyl face the enemy
        spriteObject.GetComponent<SpriteCorrector>().makeFacePlayer();

        // Move the pterodactyl to the same y level as the player while keeping {screechDistance} from the player
        Vector3 desiredVector3 = new Vector3(player.transform.position.x + screechDistance, player.transform.position.y + 0.3f);
        Vector3 moveTowardsVectorWithY = Vector3.MoveTowards(transform.position, desiredVector3, descentSpeed);
        if (!isAtVector)
            rb.MovePosition(moveTowardsVectorWithY);
      
        // If we are at or have gone to the desired vector3
        if ((desiredVector3.x - transform.position.x < 0.2 && desiredVector3.y == transform.position.y) || isAtVector)
        {
            isAtVector = true;
            if (attackTimer > 0 && !isScreeching) // Start coroutines and update screeching bool
            { // Only runs on first update
                isScreeching = true;
                StartCoroutine(ScreechDelaySpawn());
                StartCoroutine(ScreechMovement());
            }
            else if (attackTimer > 0) // Decrease attack timer if screeching
            {
                attackTimer -= Time.deltaTime;
            }
            else // Set the pterodactyl to reset if finished screeching
            {
                state = PterodactylState.Resetting;
                isAtVector = false;
            }
        }
    }

    /*
     * Helper for the BoostSwoop method. Specifically resets the pterodactyl.
     */
    private void ResetPterodactyl(float pterodactylDesiredYPosition, float yFloatingUpSpeed)
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
            isDiving = false;
            isScreeching = false;
            hitObject = false;
            StopAllCoroutines();
            StartCoroutine(Idle());
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
     * Coroutine methods that controls the BoostSwoop Attack.
     */
    IEnumerator SwoopCoroutine()
    {
        while (state == PterodactylState.Attacking || state == PterodactylState.Resetting)
        {
            BoostSwoop();
            yield return null;
        }
    }

    /**
     * Coroutine methods that controls the Dive Attack.
     */
    IEnumerator DiveCoroutiune()
    {
        while (state == PterodactylState.Attacking || state == PterodactylState.Resetting)
        {
            Dive();
            yield return null;
        }
    }

    /**
     * Delays the pterodactyls dive so it doesnt go up and come down instantly.
     */
    IEnumerator DiveDelay()
    {
        float diveSpeed = 40f;

        yield return new WaitForSeconds(0.7f);

        state = PterodactylState.Resetting;
        rb.velocity = new Vector2(0, -diveSpeed);

    }

    IEnumerator ScreechCoroutine()
    {
        while (state == PterodactylState.Attacking || state == PterodactylState.Resetting)
        {
            ScreechAttack();
            yield return null;
        }
    }

    /**
     * Delays the pterodactyl screech spawning.
     */
    IEnumerator ScreechDelaySpawn()
    {
        while (state == PterodactylState.Attacking)
        {
            GetComponentInChildren<ScreechController>().SpawnObject();
            yield return new WaitForSeconds(1f);
        }
    }

    /**
     * Deals with the movement of the pterodactyl when screeching.
     */
    IEnumerator ScreechMovement()
    {
        bool isUp = true;
        float yMovementSpeed = 6f;

        Vector3 desiredVector3 = transform.position;
        Vector3 moveTowards;
        while (state == PterodactylState.Attacking)
        {
            // if we are at the end bounds of our y level movement change the direction
            if (desiredVector3 == transform.position) 
            {
                isUp = !isUp;
                if (isUp)
                {
                    desiredVector3 = new Vector3(player.transform.position.x + screechDistance, transform.position.y + -4f);
                }
                else
                {
                    desiredVector3 = new Vector3(player.transform.position.x + screechDistance, transform.position.y + 4f);
                }
            } else
            {
                desiredVector3 = new Vector3(player.transform.position.x + screechDistance, desiredVector3.y);
            }

            // Move
            moveTowards = Vector3.MoveTowards(transform.position, desiredVector3, yMovementSpeed * Time.fixedDeltaTime);
            rb.MovePosition(moveTowards);
            yield return null;
        }
    }
}
