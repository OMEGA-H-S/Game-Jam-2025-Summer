using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] [Tooltip("lower val")] private float startVert;
    [SerializeField] [Tooltip("upper val")] private float endVert;
    [SerializeField] [Tooltip("left val")] private float startHoriz;
    [SerializeField] [Tooltip("right val")] private float endHoriz;

    [SerializeField] private float platformSpeed = 4f;
    [SerializeField] private float directionCooldown = 1f;

    [SerializeField] private bool moveVertically;
    [SerializeField] private bool moveHorizically;


    // Start is called before the first frame update
    void Start()
    {
        if (moveVertically)
        {
            StartCoroutine(vertMovement(true));
        }

        if (moveHorizically)
        {
            StartCoroutine(horizontalMovement(true));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator vertMovement(bool goingUp) 
    {
        while(true)
        {
            if (goingUp)
            {
                while (transform.localPosition.y < endVert)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, 
                        transform.localPosition.y + platformSpeed * Time.deltaTime, 0);
                    yield return null;
                }
                goingUp = false;
                yield return new WaitForSeconds(directionCooldown);

            }
            else
            {
                while (transform.localPosition.y > startVert)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, 
                        transform.localPosition.y - platformSpeed * Time.deltaTime, 0);
                    yield return null;
                }
                goingUp = true;
                yield return new WaitForSeconds(directionCooldown);
            }
        }
        
    }

    private IEnumerator horizontalMovement(bool goingRight)
    {
        while (true)
        {
            if (goingRight)
            {
                while (transform.localPosition.x < endHoriz)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + platformSpeed * Time.deltaTime, 
                        transform.localPosition.y, 0);
                    yield return null;
                }
                goingRight = false;
                yield return new WaitForSeconds(directionCooldown);

            }
            else
            {
                while (transform.localPosition.x > startHoriz)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x - platformSpeed * Time.deltaTime, 
                        transform.localPosition.y, 0);
                    yield return null;
                }
                goingRight = true;
                yield return new WaitForSeconds(directionCooldown);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.transform.parent = this.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }


}
