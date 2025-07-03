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

    [SerializeField] private Vector2 crouchOffset, standingOffset;
    [SerializeField] private Vector2 crouchSize, standingSize;
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip jumpSound;
    bool isWalking = false;
    private BoxCollider2D collider;

    private PlayerHealth anim;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        standingOffset = collider.offset;
        standingSize = collider.size;
        anim = GetComponent<PlayerHealth>();    
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
        //Debug.Log(gunPivot);

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

        /*
        //Logic for crouching: player cannot shoot while crouching
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            this.transform.Rotate(new Vector3(0, 0, 90) * transform.localScale.x / Mathf.Abs(transform.localScale.x));
            Debug.Log("Transform rotation: " + transform.eulerAngles.z);
            Debug.Log("Rigidbody rotation: " + body.rotation);
        }
        
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            this.transform.Rotate(new Vector3(0, 0, 90) * transform.localScale.x / Mathf.Abs(transform.localScale.x));
            gunPivot.SetActive(true);
        }
        */
        crouch();

    }

    private void FixedUpdate()
    {
        float input = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(input * speed, body.velocity.y);
        if(Mathf.Abs(input) > 0.01f)
        {
            anim.playerMoving(true);
            
        }
        else
        {
            isWalking = false;
            StopAllCoroutines();
            anim.playerMoving(false);

        }
        

        if (pendingJump)
        {
            SoundEffectsManager.instance.PlaySoundEffectClip(jumpSound, transform, 1f);
            body.velocity = new Vector2(body.velocity.x, jumpHeight);
            pendingJump = false;
        
        }
        
    }

    private bool isGrounded()
    {
        //Add logic to determine when player is grounded
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        RaycastHit2D cast = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        //Debug.Log(cast.collider.gameObject.layer + " " + LayerMask.NameToLayer("Ground"));
        return cast.collider != null;
    }

    private void crouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            collider.size = this.crouchSize;
            collider.offset = this.crouchOffset;
            gunPivot.SetActive(false);
            anim.playerCrouching(true);
        }

        //Add crouching animatino

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            collider.size = this.standingSize;
            collider.offset = this.standingOffset;
            gunPivot.SetActive(true);
            anim.playerCrouching(false);

        }
    }









    IEnumerator WalkingAudio()
    {
        while (isWalking && isGrounded()) {
            SoundEffectsManager.instance.PlaySoundEffectClip(walkingSound, transform, 1f);
            yield return new WaitForSeconds(walkingSound.length);
        }
    }
}
