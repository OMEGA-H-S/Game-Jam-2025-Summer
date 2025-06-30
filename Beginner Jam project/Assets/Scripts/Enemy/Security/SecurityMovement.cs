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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        centerLocation = this.transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gunPivot.gameObject.SetActive(false);
        collider = GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(currMoveState);
        //Part 1: Idle
        switch (currMoveState)
        {
            case MovementState.Idle:

                if (isAtLocation)
                {
                    Debug.Log("Running this part of the code now");
                    if (cooldownTime >= idleMovementCooldown)
                    {

                        //Determine a random location
                        Vector3 randomLoc = GetRandomLocation();
                        isAtLocation = false;
                        //Move towards that position 
                        MoveTowards(randomLoc);
                    }
                    else
                    {
                        //Debug.Log("Here");
                        cooldownTime += Time.deltaTime;
                    }
                }
                if(Vector2.Distance(this.transform.position, player.position) < targetDistance)
                {
                    currMoveState = MovementState.Following;
                    playerFound();
                    gunPivot.gameObject.SetActive(true);
                    rb.velocity = Vector2.zero;
                }
                break;

            case MovementState.Following:
                //Debug.Log("Player found...Attack!");
                Debug.Log("Here");
                if (Vector2.Distance(this.transform.position, player.position) > targetDistance)
                {
                    
                    gunPivot.gameObject.SetActive(false);
                    isAtLocation = true;
                    cooldownTime = 0;   //Reset the cooldown time in case the enemy was in the middle of a cooldown when it saw the player
                    currMoveState = MovementState.Idle;
                }
                //Move one unit towards player
                playerFound();
                if(!isAttacking)
                {
                    Debug.Log("Attacking now");
                    attack();
                    isAttacking = true;
                }
                dodgeAttack();
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
        StartCoroutine(MoveTillComplete(location));
    }

    private IEnumerator MoveTillComplete(Vector3 location)
    {
        Vector3 distance = location - transform.position;
        while (distance.magnitude > 0.5f)
        {
            yield return new WaitForFixedUpdate();
            Vector3 rbPos = rb.position;
            distance = location - rbPos;
            Debug.Log("Still here " + rb.velocity);
        }
        isAtLocation = true;
        cooldownTime = 0;
        rb.velocity = Vector2.zero;
        //Create a random range to allow for variation in the enemies movement
        idleMovementCooldown = Random.Range(0.5f, 1.5f);
    }

    private void attack()
    {
        //chargeAttack();
        //StartCoroutine(chargeAttack());
        //mechanics for attack: 
        //Attack 1: Shoot at player
        //Attack 2: Throw a grenade
        //Detect opponent attacks, jump up 50% of the time
        //Attack 3: Tackle (Run at opponent, can still be damaged but can only be stopped by dodging
        //Inform other nearby security guards
        StartCoroutine(shootAttack());
    }

    private IEnumerator shootAttack()
    {
        //Vector3 playerPos = player.position;
        //Debug.Log("Gun pivot: " + gunPivot);
        //Debug.Log(gunPivot.GetComponentInChildren<SecurityGunController>());

        Debug.Log("Shooting");
        gunPivot.GetComponentInChildren<SecurityGunController>().SpawnLaser();
        yield return new WaitForSeconds(2);
        isAttacking = false;

    }

    private IEnumerator grenadeAttack()
    {
        gunPivot.gameObject.SetActive(false);
        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        //grenade.GetComponent<Rigidbody2D>().velocity = new Vector3(5, 5, 0);
        grenade.GetComponent<GrenadeController>().AimAtPlayer();

        yield return new WaitForSeconds(2);
        isAttacking = false;
        gunPivot.gameObject.SetActive(true);
    }

    private IEnumerator chargeAttack()
    {
        //Debug.Log("Here!");

        gunPivot.gameObject.SetActive(false);
        float distance = player.position.x - transform.position.x;
        Vector2 target = new Vector2(distance * 2 + transform.position.x, transform.position.y);    //Go to the same distance, but on the other side of the player
        //Debug.Log(target);

        bool doneChargeAttack = false;
        Vector2 moveIncrement = target.x < transform.position.x ? Vector2.left : Vector2.right;
        
        while (!doneChargeAttack)
        {
            transform.position = transform.position + (Vector3)moveIncrement * Time.deltaTime * speed * 2;
            if(Vector2.Distance(target, transform.position) < 0.2f)
            {
                Debug.Log("Reached the target");

                doneChargeAttack = true;
            }

            if (collider.IsTouching(player.GetComponent<Collider2D>()))
            {
                //If so, deal damage to player
                
                Debug.Log("Player took 10 damage from colliding with the enemy");
                doneChargeAttack = true;

            }

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
        isAttacking = false;
        gunPivot.gameObject.SetActive(true);
        //Add a little cooldown
        yield return new WaitForSeconds(1);

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
            }
        }
    }


}
