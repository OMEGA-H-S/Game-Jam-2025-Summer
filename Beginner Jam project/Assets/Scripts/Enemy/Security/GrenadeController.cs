using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float throwStrength;
    private Rigidbody2D rb;
    [SerializeField] private float grenadeHeight;
    private Animator animator;

    [SerializeField] private float blastRadius = 12f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //Debug.Log(player == null);
        animator = GetComponent<Animator>();
        
    }
    public void AimAtPlayer()
    {
        //Debug.Log(player == null);
        float distance = Vector3.Distance(player.position, transform.position);
        if(player.position.x < transform.position.x)
        {
            rb.velocity = new Vector2(Random.Range(-distance * throwStrength * 0.9f, -distance * throwStrength * 1.1f), 
                grenadeHeight);
        }
        else
        {
            rb.velocity = new Vector2(Random.Range(distance * throwStrength * 0.9f, distance * throwStrength * 1.1f),
                grenadeHeight);
        }

        Debug.Log(rb.velocity);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Ground")
        {
            animator.SetTrigger("BombTriggered");
            //Start explosion sequence
        }

    }

    private void Explosion()
    {
        if(Vector2.Distance(player.transform.position, transform.position) < blastRadius)
        {
            //Eventually, add damage to player
            player.GetComponent<PlayerHealth>().playerAttacked((int)Random.Range(4, 7));
        }
        //Instantiate a separate game object that will just explode (using unity animation)
        GameObject.Destroy(this.gameObject);
    }

    private void aboutToExplode()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        transform.localScale = new Vector3(8, 8, 8);
    }
}
