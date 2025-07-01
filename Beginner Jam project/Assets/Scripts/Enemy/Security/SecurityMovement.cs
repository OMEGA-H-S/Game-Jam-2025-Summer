using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class SecurityMovement : MonoBehaviour
{
    [SerializeField][Range(0, 10f)] private float speed = 2.0f;
    private Rigidbody2D rb;
    private Vector3 centerLocation;
    
    private enum MovementState
    {
        Idle,
        Following
    };
    private MovementState currMoveState = MovementState.Idle;
    [SerializeField] private float radius = 7.0f;
    private bool isAtLocation = true;
    [SerializeField] private float idleMovementCooldown = 0.5f;
    private float cooldownTime;
    private Transform player;
    [SerializeField] private float targetDistance = 8f;

    [SerializeField] private Transform gunPivot;

    [SerializeField] private GameObject grenadePrefab;
    private Collider2D collider;

    [SerializeField] private LayerMask bulletLayer;

    private bool isAttacking = false;

    [SerializeField] private float jumpHeight = 5.0f;

    [SerializeField] private float dodgeRange = 10f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] [Range(0, 1)] private float dodgeFreq = 0.3f;
    [SerializeField] private float attackDelay = 1f;

    private Animator anim;

    private enum Difficulty
    {
        Easy, 
        Medium, 
        Hard
    };

    [SerializeField] Difficulty guardDifficulty;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        centerLocation = this.transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gunPivot.gameObject.SetActive(false);
        collider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        if(guardDifficulty == Difficulty.Easy)
        {
            radius = 4f;
            targetDistance = 16;
            speed = 2f;
            jumpHeight = 10f;
            dodgeFreq = 0.25f;
            attackDelay = 1.2f;
            anim.SetTrigger("Green");
        }
        if(guardDifficulty == Difficulty.Medium)
        {
            this.radius = 7f;
            targetDistance = 18f;
            speed = 4.92f;
            jumpHeight = 18f;
            dodgeFreq = 0.45f;
            attackDelay = 1f;
            anim.SetTrigger("Blue");

        }
        else if (guardDifficulty == Difficulty.Hard)
        {
            this.radius = 10f;
            targetDistance = 22f;
            speed = 6f;
            jumpHeight = 22f;
            dodgeFreq = 0.85f;
            attackDelay = 0.8f;

        }

        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(currMoveState);
        //Part 1: Idle
        switch (currMoveState)
        {
            case MovementState.Idle:

                if (isAtLocation)
                {
                    //Debug.Log("Running this part of the code now");
                    if (cooldownTime >= idleMovementCooldown)
                    {

                        //Determine a random location
                        Vector3 randomLoc = GetRandomLocation();
                        isAtLocation = false;
                        //Move towards that position 
                        MoveTowards(randomLoc);
                        anim.SetBool("isWalking", true);
                    }
                    else
                    {
                        //Debug.Log("Here");
                        cooldownTime += Time.deltaTime;
                        anim.SetBool("isWalking", false);
                    }
                }
                if(Vector2.Distance(this.transform.position, player.position) < targetDistance)
                {
                    currMoveState = MovementState.Following;
                    isAttacking = false;
                    playerFound();
                    gunPivot.gameObject.SetActive(true);
                    rb.velocity = Vector2.zero;
                    //Debug.Log("I see the player, so i set my velocity to " + rb.velocity);
                    anim.SetBool("isWalking", false);
                }
                break;

            case MovementState.Following:
                //Debug.Log("Player found...Attack!");
                //Debug.Log("Here");
                //Debug.Log("Check velocity here: " + rb.velocity);
                if (Vector2.Distance(this.transform.position, player.position) > targetDistance && !isAttacking && isGrounded())
                {
                    
                    gunPivot.gameObject.SetActive(false);
                    isAtLocation = true;
                    cooldownTime = 0;   //Reset the cooldown time in case the enemy was in the middle of a cooldown when it saw the player
                    currMoveState = MovementState.Idle;
                }
                //Move one unit towards player

                if(!isAttacking && currMoveState == MovementState.Following)
                {
                    playerFound();
                    //Debug.Log("Attacking now");
                    attack();
                    isAttacking = true;
                }
                
                break;

        }
        //Part 2: Check if player is within vicinity
        //If so, check if player is within the angle chosen to be the enemy's field of view
        //If both are true, change states and follow the player
        //At each fixedupdate, the velocity of the enemy should be in the direction of the player. 
        //If the player is no longer visible, the enemy should head to the spot at which the player was last seen
        //If the player is still not within the line of sight, then go back to idle movement, with the new center being that point
    }
    private void playerFound()
    {

        if (player.transform.position.x < this.transform.position.x && transform.localScale.x > 0)
        {
            //Debug.Log("Here1");
            //Face left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            if (gunPivot.gameObject.activeInHierarchy)
            {
                gunPivot.GetComponentInChildren<SecurityGunController>().enemySwitchedDirection();
            }


        }
        else if(player.transform.position.x > this.transform.position.x && transform.localScale.x < 0)
        {
            //Debug.Log("Here2");
            //Face right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            if (gunPivot.gameObject.activeInHierarchy)
            {
                gunPivot.GetComponentInChildren<SecurityGunController>().enemySwitchedDirection();
            }

        }

        if (gunPivot.gameObject.activeInHierarchy)
        {
            gunPivot.GetComponentInChildren<SecurityGunController>().adjustGunComparison();
        }

    }

    private Vector3 GetRandomLocation()
    {
        float x = Random.Range(-radius, radius) + centerLocation.x;
        float y = this.transform.position.y;
        return new Vector3(x, y, 0);
    }

    private void MoveTowards(Vector3 location)
    {
        //Adjust direction
        if (location.x > transform.position.x)
        {
            //Face left
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            if (gunPivot.gameObject.activeInHierarchy)
            {
                //gunPivot.GetComponentInChildren<SecurityGunController>().enemySwitchedDirection();
            }

        }
        else
        {
            //Face right
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            if (gunPivot.gameObject.activeInHierarchy) { 
                //gunPivot.GetComponentInChildren<SecurityGunController>().enemySwitchedDirection();
            }
        }
        Vector3 direction = (location - transform.position).normalized;

        rb.velocity = direction * speed;
        //Keep going until we are at the position
        StartCoroutine(MoveTillComplete(location, direction));
    }

    private IEnumerator MoveTillComplete(Vector3 location, Vector3 direction)
    {
        Vector3 distance = location - transform.position;
        while (distance.magnitude > 0.5f && currMoveState == MovementState.Idle)
        {
            rb.velocity = direction * speed;
            yield return new WaitForFixedUpdate();
            Vector3 rbPos = rb.position;
            distance = location - rbPos;
            //Debug.Log("Still here " + rb.velocity);
        }
        isAtLocation = true;
        cooldownTime = 0;
        rb.velocity = Vector2.zero;
        //Create a random range to allow for variation in the enemies movement
        idleMovementCooldown = Random.Range(0.5f, 1.5f);
    }

    private void attack()
    {
        Debug.Log("Looks like im attacking again");
        if(Random.Range(0f, 1f) < dodgeFreq)
        {
            dodgeAttack();
            Debug.Log("Dodging Attack");
        }
        float whichAttack = Random.Range(0f, 1f);
        if(whichAttack <= 0.85f)
        {
            StartCoroutine(shootAttack());
            Debug.Log("Shooting");
        }
        else if(whichAttack < 0.95f)
        {
            StartCoroutine(grenadeAttack());
            Debug.Log("Throwing grenade");
        }
        else
        {
            StartCoroutine(chargeAttack());
            Debug.Log("Charging");
        }
        //chargeAttack();
        //StartCoroutine(chargeAttack());
        //mechanics for attack: 
        //Attack 1: Shoot at player
        //Attack 2: Throw a grenade
        //Detect opponent attacks, jump up 50% of the time
        //Attack 3: Tackle (Run at opponent, can still be damaged but can only be stopped by dodging
        //Inform other nearby security guards
        
    }

    private IEnumerator shootAttack()
    {
        //Vector3 playerPos = player.position;
        //Debug.Log("Gun pivot: " + gunPivot);
        //Debug.Log(gunPivot.GetComponentInChildren<SecurityGunController>());

        Debug.Log("Shooting");
        gunPivot.GetComponentInChildren<SecurityGunController>().SpawnLaser();
        yield return new WaitForSeconds(0.5f * attackDelay);
        isAttacking = false;

    }

    private IEnumerator grenadeAttack()
    {
        gunPivot.gameObject.SetActive(false);
        GameObject grenade = Instantiate(grenadePrefab, transform.position + (Vector3)Vector2.up * 2, transform.rotation);
        //grenade.GetComponent<Rigidbody2D>().velocity = new Vector3(5, 5, 0);
        grenade.GetComponent<GrenadeController>().AimAtPlayer();

        yield return new WaitForSeconds(3 * attackDelay);
        isAttacking = false;
        gunPivot.gameObject.SetActive(true);
    }

    private IEnumerator chargeAttack()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isCharging", true);
        //Debug.Log("Here!");

        gunPivot.gameObject.SetActive(false);
        float distance = player.position.x - transform.position.x;
        Vector2 target = new Vector2(distance * 2 + transform.position.x, transform.position.y);    //Go to the same distance, but on the other side of the player
        //Debug.Log("Target: " + target);

        bool doneChargeAttack = false;
        Vector2 moveIncrement = target.x < transform.position.x ? Vector2.left : Vector2.right;
        float abortTime = 0f;

        while (!doneChargeAttack)
        {
            transform.position = transform.position + (Vector3)moveIncrement * Time.deltaTime * speed * 2;
            if(Vector2.Distance(target, transform.position) < 0.2f)
            {
                Debug.Log("Reached the target");

                doneChargeAttack = true;
            }

            if (collider.IsTouching(player.GetComponent<BoxCollider2D>()))
            {
                //If so, deal damage to player

                player.GetComponent<PlayerHealth>().playerAttacked((int)Random.Range(5, 10));
                doneChargeAttack = true;

            }
            abortTime += Time.deltaTime;
            if(abortTime > 3f)
            {
                doneChargeAttack = true;
            }

            /*

            RaycastHit2D checkForWalls = Physics2D.BoxCast(collider.bounds.center,
                new Vector2(collider.bounds.size.x * 1.25f, collider.bounds.extents.y),
                0, Vector2.zero, 0f);
            if (checkForWalls.collider != null && checkForWalls.collider.tag != "Player" && checkForWalls.collider.tag != "Enemy");
            {
                //This means something is blocking it, so just end the loop
                doneChargeAttack = true;
            }
            */

            yield return null;

        }
        //isAtLocation = true;
        //cooldownTime = 0;
        //rb.velocity = Vector2.zero;
        //Create a random range to allow for variation in the enemies movement
        //idleMovementCooldown = Random.Range(0.5f, 1.5f);
        /*
        rb.velocity = new Vector2(transform.localScale.x, 0).normalized * speed * 2;
        StartCoroutine(MoveTillComplete(endGoal));
        while(!isAtLocation)
        {
            //Check if player is touching the guard
            if(collider.IsTouching(player.GetComponent<Collider2D>())) {
                //If so, deal damage to player
                Debug.Log("Player took 10 damage from colliding with the enemy");
                isAtLocation = true;

            }
            yield return null;
        }
        */
        //yield return new WaitForSeconds(2);

        Debug.Log("Charge attack finished");
        anim.SetBool("isCharging", false);
        yield return new WaitForSeconds(2 * attackDelay);
        isAttacking = false;
        gunPivot.gameObject.SetActive(true);
        //Add a little cooldown
        
    }

    private void dodgeAttack()
    {
        Vector2 direction = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        
        RaycastHit2D playerBullet = Physics2D.BoxCast(this.collider.bounds.center, collider.bounds.size, 0, direction, dodgeRange, bulletLayer);
        
        if(playerBullet.collider != null)
        {
            
            if(playerBullet.collider.gameObject.GetComponent<LaserController>().getOwner() == LaserController.Owner.Player)
            {
                rb.velocity = Vector2.up * jumpHeight;
                //Debug.Log("Velocity after jumping: " + rb.velocity);
            }
        }
    }

    private bool isGrounded()
    {
        //Add logic to determine when player is grounded
        Collider2D collider = GetComponent<BoxCollider2D>();
        RaycastHit2D cast = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        //Debug.Log(cast.collider.gameObject.layer + " " + LayerMask.NameToLayer("Ground"));
        return cast.collider != null;
    }


}
