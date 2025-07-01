using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : MonoBehaviour
{
    [SerializeField] private int numInDirection = 10;
    private int numGoneInDirection = 0;
    private float increment = 0.05f;
    [SerializeField] private float timeBetweenMovement = 0.01f;
    [SerializeField] private int keyCode;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(moveKey());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator moveKey()
    {
        while (true)
        {
            if (numGoneInDirection < numInDirection)
            {
                transform.Translate(0, increment, 0);
                numGoneInDirection++;
                yield return new WaitForSeconds(0.04f);
            }
            else
            {
                increment *= -1;
                numGoneInDirection = 0;
                yield return new WaitForSeconds(0.04f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Communicate with player collectible script
        if (collision.gameObject.name == "Player")
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            player.playerAttacked(-10);
            GameObject.Destroy(this.gameObject);
        }

    }
}
