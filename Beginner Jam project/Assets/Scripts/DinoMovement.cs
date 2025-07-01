using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float changeDirectionTime = 2f;
    public LayerMask visionMask;

    private float timer;
    private int direction; // -1 = left, 1 = right
    private Rigidbody2D rb;
    private GameObject player;

    public bool hasLineOfSight = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        PickNewDirection();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (!hasLineOfSight)
        {
            if (timer <= 0)
            {
                PickNewDirection();
            }

            // Apply movement
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

            // Flip by scaling
            if (direction < 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (direction > 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        
    }

    void PickNewDirection()
    {
        direction = Random.Range(0, 2) == 0 ? -1 : 1;
        timer = Random.Range(1f, changeDirectionTime);
    }

    private void FixedUpdate()
    {
        Vector2 dir = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        Vector2 origin = transform.position;
        RaycastHit2D ray = Physics2D.Raycast(origin, dir, 10f, visionMask);
        
        Debug.DrawRay(origin, dir * 10f, Color.red);
        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player");
            Debug.Log(hasLineOfSight);
        }
        else
        {
            hasLineOfSight = false;
        }
        
    }
}
