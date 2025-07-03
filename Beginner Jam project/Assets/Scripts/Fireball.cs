using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float projectileSpeed = 3f;
    private Rigidbody2D rigidBody;

    private void Start()
    {


    }
    public void Launch(Vector2 direction)
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = direction.normalized * projectileSpeed;

        // Optional: flip the sprite if going left
        if (direction.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }


}