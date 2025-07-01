using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCorrector : MonoBehaviour
{
    [SerializeField] GameObject movementObject;
    [SerializeField] GameObject player;
    Rigidbody2D rb;

    private void Start()
    {
        rb = movementObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.x <= -1)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void makeFacePlayer()
    {
        float direction = movementObject.transform.position.x - player.transform.position.x;

        if (direction < 0)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
