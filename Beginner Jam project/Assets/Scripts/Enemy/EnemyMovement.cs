using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] [Range(0, 10f)] private float speed = 2.0f;
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
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        centerLocation = this.transform.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Part 1: Idle
        switch (currMoveState)
        {
            case MovementState.Idle:
                if (isAtLocation) 
                {
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
                        Debug.Log("Here");
                        cooldownTime += Time.deltaTime;
                    }
                }
                break;
            //case MovementState.Following:

        }
        

        //Part 2: Check if player is within vicinity
        //If so, check if player is within the angle chosen to be the enemy's field of view
        //If both are true, change states and follow the player
        //At each fixedupdate, the velocity of the enemy should be in the direction of the player. 
        //If the player is no longer visible, the enemy should head to the spot at which the player was last seen
        //If the player is still not within the line of sight, then go back to idle movement, with the new center being that point
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
        if(location.x > transform.position.x)
        {
            //Face left
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            //Face right
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        Vector3 direction = (location - transform.position).normalized;
        rb.velocity = direction * speed;
        //Keep going until we are at the position
        StartCoroutine(MoveTillComplete(location));
    }

    private IEnumerator MoveTillComplete(Vector3 location) 
    {
        Vector3 distance = location - transform.position;
        while(distance.magnitude > 0.5f)
        {
            yield return new WaitForFixedUpdate();
            Vector3 rbPos = rb.position;
            distance = location - rbPos;
        }
        isAtLocation = true;
        cooldownTime = 0;
        rb.velocity = Vector2.zero;
        //Create a random range to allow for variation in the enemies movement
        idleMovementCooldown = Random.Range(0.5f, 1.5f);
    }
}
