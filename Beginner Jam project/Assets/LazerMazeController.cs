using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerMazeController : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().playerAttacked(3);
        }
    }

    public void clearLaserTriggers()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }

    public void activateLaserTriggers()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = true;
    }


}
