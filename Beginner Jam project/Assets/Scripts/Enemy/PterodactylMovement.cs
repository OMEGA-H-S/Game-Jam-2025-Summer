using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PterodactylMovement : MonoBehaviour
{
    [Header("Necessities")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject spriteObject;

    [Space(10)]
    [Header("Generalized Attack Values")]
    [SerializeField] private float damageTaken = 10f;

    [Space(10)]
    [Header("Generalized Attack Values")]
    [Tooltip("Determines how far off of an x or y value can be for all attacks to start")]
    [SerializeField] private float attackError = 0.2f;
    [Tooltip("The Speed that the Pterodactyl resets to the Y position")]
    [SerializeField] private float yFloatingUpSpeed = 10f;


    [Space(10)]
    [Header("Swoop Values")]
    [Tooltip("The amount of time the swoop attack has before resetting")]
    [SerializeField] private float swoopAttackTime = 1f;
    [SerializeField] private float boostSpeed = 60f;
    [SerializeField] private float descendSpeed = 0.1f;
    [SerializeField] private float playerSwoopDamage = 10f;

    [Space(10)]
    [Header("Dive Values")]
    [Tooltip("Value the pterodactyl needs to get up to before diving")]
    [SerializeField] private float aboveYValue = 17f; 
    [SerializeField] private float ascentSpeed = 5f;
    [Tooltip("The amount of time the dive attack has before resetting")]
    [SerializeField] private float diveAttackTime = 5f;
    [SerializeField] private float diveSpeed = 100f;
    [SerializeField] private float timeBeforeDiving = 0.7f;
    [SerializeField] private float playerDiveDamage = 30f;

    [Space(10)]
    [Header("Screech Values")]
    [SerializeField] private float descentSpeed = 0.4f;
    [Tooltip("The amount of time the screech attack has before resetting")]
    [SerializeField] private float screechAttackTime = 5f;
    [SerializeField] private float screechDistance = 15f; 
    [SerializeField] private float screechTime = 1.5f;
    [Tooltip("Speed that the pterodactyl moves on the y while atatacking")]
    [SerializeField] private float yMovementSpeed = 8f;
    
    [Space(10)]
    [Header("Idle Values")]
    [SerializeField] private float idleHorizontalSpeed = 5f;

    private Rigidbody2D rb;

    private PterodactylState state;
    private enum PterodactylState
    {
        Idle,
        Attacking,
        Resetting
    }

    private Attacks attack;
    private enum Attacks
    {
        Swoop,
        Dive,
        Screech,
        None
    }
    
    // Utility Variables
    private float attackTimer;
    bool hitObject;
    bool isSwooping;
    bool isDiving;
    bool isScreeching;
    bool isAtVector;
    float pterodactylDesiredYPosition; // Holds the YValue that the pterodactyl floats at

    private void Start()
    {
        attackTimer = swoopAttackTime;
        rb = GetComponent<Rigidbody2D>();
        state = PterodactylState.Idle;
        hitObject = false;
        pterodactylDesiredYPosition = transform.position.y;
        attack = Attacks.None;
        StartCoroutine(Idle());
    }

    private void FixedUpdate()
    {
        float maximumAttackDistance = 15f; // Determines how far the player can be for an attack

        // if the player is within the max attack distance
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < maximumAttackDistance || state != PterodactylState.Idle)

            if (Random.Range(0, 50) == 1 && state == PterodactylState.Idle)
            {
                int random = Random.Range(0,3);

                // TODO Placeholder for which attack the pterodactyl does
                if (random == 0)
                {
                    attackTimer = swoopAttackTime;
                    StopCoroutine(Idle());
                    state = PterodactylState.Attacking;
                    attack = Attacks.Swoop;
                    StartCoroutine(SwoopCoroutine()); // Start Swooping
                    GetComponentInChildren<SpriteCorrector>().startSwoop();
                }
                else if (random == 1)
                {
                    attackTimer = diveAttackTime;
                    state = PterodactylState.Attacking;
                    attack = Attacks.Dive;
                    StartCoroutine(DiveCoroutiune());

                }
                else
                {
                    GetComponentInChildren<SpriteCorrector>().startScreech();
                    attackTimer = screechAttackTime;
                    state = PterodactylState.Attacking;
                    attack = Attacks.Screech;
                    StartCoroutine(ScreechCoroutine());
                }
            }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (attack == Attacks.Swoop)
                player.GetComponent<PlayerHealth>().playerAttacked(playerSwoopDamage);
            else if (attack == Attacks.Dive)
                player.GetComponent<PlayerHealth>().playerAttacked(playerDiveDamage);
        }

        hitObject = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
            GetComponent<EnemyHealth>().enemyAttacked(damageTaken);

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
                    GetComponentInChildren<SpriteCorrector>().endSwoop();
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
        float aboveGroundSwoopYLevel = 0.3f;

        // Moves the pterodactyl to the correct y position of where the player is
        Vector3 verticalMovementVector = new Vector3(transform.position.x, player.transform.position.y + aboveGroundSwoopYLevel);
        Vector3 newPosition = Vector3.MoveTowards(transform.position, verticalMovementVector, descendSpeed);
        rb.MovePosition(newPosition);
        
        // If the transform has gotten to where it needs to go
        if ((Mathf.Abs(transform.position.y - verticalMovementVector.y) < attackError))
        {
            state = PterodactylState.Resetting; // Set the pterodactyl to reset
            
            // Finds the direction the pterodactyl needs to fling towards and "boosts"
            float direction = (player.transform.position - transform.position).x / Mathf.Abs((player.transform.position - transform.position).x);
            rb.velocity = new Vector3(direction * boostSpeed, 0);
            GetComponentInChildren<SpriteCorrector>().endSwoop();
        }
    }

    /**
     * Deals with Mechancs for the Dive attack.
     */
    private void Dive()
    {
        // if attacking
        if (state == PterodactylState.Attacking && !hitObject)
        {

            // Move to the y position over the player
            Vector3 desiredDiveVector3 = new Vector3(player.transform.position.x, aboveYValue);
            Vector3 diveVector = Vector3.MoveTowards(transform.position, desiredDiveVector3, ascentSpeed * Time.deltaTime);
            if (!isDiving) {
                rb.MovePosition(diveVector);
            }

            // If we've reached the hight and we aren't diving yet
            if ((Mathf.Abs(transform.position.x - desiredDiveVector3.x) < attackError && transform.position.y == desiredDiveVector3.y) && !isDiving)
            {
                // stop the movement, and delay the dive
                rb.velocity = Vector3.zero;
                isDiving = true;
                StartCoroutine(DiveDelay()); // Starts the dive
            }
        } else
        {
            // If the attack timer is done, reset the pteroactyl
            if (attackTimer <= 0 || hitObject) {
                GetComponentInChildren<SpriteCorrector>().endDive();
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
            GetComponentInChildren<SpriteCorrector>().endScreech();
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
        // Make the Pterodactyl face the enemy
        spriteObject.GetComponent<SpriteCorrector>().makeFacePlayer();

        // Move the pterodactyl to the same y level as the player while keeping {screechDistance} from the player
        Vector3 desiredVector3 = new Vector3(player.transform.position.x + screechDistance, player.transform.position.y);
        Vector3 moveTowardsVectorWithY = Vector3.MoveTowards(transform.position, desiredVector3, descentSpeed);
        if (!isAtVector)
            rb.MovePosition(moveTowardsVectorWithY);
      
        // If we are at or have gone to the desired vector3
        if ((desiredVector3.x - transform.position.x < attackError && desiredVector3.y == transform.position.y) || isAtVector)
        {
            isAtVector = true;
            if (attackTimer > 0 && !isScreeching) // Start coroutines and update screeching bool
            { // Only runs on first update
                isScreeching = true;
                StartCoroutine(ScreechMovementSpawn());
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
            attackTimer = swoopAttackTime;
            state = PterodactylState.Idle;
            isSwooping = false;
            isDiving = false;
            isScreeching = false;
            hitObject = false;
            attack = Attacks.None;
            StopAllCoroutines();
            StartCoroutine(Idle());
        }
    }


    /**
     * Coroutine methods that makes the pterodactyl move left or right every 2 seconds.
     */
    IEnumerator Idle()
    {
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
        yield return new WaitForSeconds(timeBeforeDiving);

        state = PterodactylState.Resetting;
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector2(0, - diveSpeed), ForceMode2D.Impulse);
        GetComponentInChildren<SpriteCorrector>().startDive();

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
     * Deals with the movement of the pterodactyl when screeching.
     */
    IEnumerator ScreechMovementSpawn()
    {
        bool isUp = true;
        float timer = screechTime;

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

            if (timer <= 0)
            {
                timer = screechTime;
                GetComponentInChildren<ScreechController>().SpawnObject(isUp, pterodactylDesiredYPosition);
            }
            else
                timer -= Time.deltaTime;

                // Move
                moveTowards = Vector3.MoveTowards(transform.position, desiredVector3, yMovementSpeed * Time.fixedDeltaTime);
            rb.MovePosition(moveTowards);
            yield return null;
        }
    }
}
