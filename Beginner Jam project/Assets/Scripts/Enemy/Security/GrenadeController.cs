using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float throwStrength;
    private Rigidbody2D rb;
    [SerializeField] private float grenadeHeight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log(player == null);
    }
    public void AimAtPlayer()
    {
        //Debug.Log(player == null);
        float distance = Vector3.Distance(player.position, transform.position);
        if(player.position.x < transform.position.x)
        {
            rb.velocity = new Vector3(-distance * throwStrength, grenadeHeight * throwStrength);
        }

    }
}
