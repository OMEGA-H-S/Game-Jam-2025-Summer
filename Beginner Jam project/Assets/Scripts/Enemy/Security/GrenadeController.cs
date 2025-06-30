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
            rb.velocity = new Vector3(-distance * Random.Range(throwStrength - 0.3f, throwStrength + 0.5f), 
                grenadeHeight * Random.Range(throwStrength - 0.3f, throwStrength + 0.3f));
        }

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
            Debug.Log("Player lost 5 health");
        }
        //Instantiate a separate game object that will just explode (using unity animation)
        GameObject.Destroy(this.gameObject);
    }
}
