using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private GameObject gunPivot;
    private Rigidbody2D body;
    private bool pendingJump = false;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float input = Input.GetAxis("Horizontal");

        /*
        float input = Input.GetAxis("Horizontal");
        float jump = 0;
        if(Input.GetKeyDown("space"))
        {
            jump = jumpHeight;
        }
        Vector3 dir = new Vector3(input * speed, jump, 0) * Time.deltaTime;
        transform.Translate(dir);
        */
        if (input < -0.1f && this.transform.localScale.x > 0)
        {
            this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
            gunPivot.GetComponentInChildren<GunController>().playerSwitchedDirection(); //Tells gun that player switched direction
        }
        else if (input > 0.1f && this.transform.localScale.x < 0)
        {
            this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
            gunPivot.GetComponentInChildren<GunController>().playerSwitchedDirection();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            pendingJump = true;
        }

    }

    private void FixedUpdate()
    {
        float input = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(input * speed, body.velocity.y);
        

        if (pendingJump)
        {
            body.velocity = new Vector2(body.velocity.x, jumpHeight);
            pendingJump = false;
        
        }
        
    }

    private bool isGrounded()
    {
        //Add logic to determine when player is grounded
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        RaycastHit2D cast = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);

        return cast.collider != null;
    }









}
