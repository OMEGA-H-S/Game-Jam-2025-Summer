using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCorrector : MonoBehaviour
{
    [SerializeField] GameObject movementObject;
    [SerializeField] GameObject player;
    [SerializeField] Animator animator;
    Rigidbody2D rb;
    bool isFlapping;

    private void Start()
    {
        rb = movementObject.GetComponent<Rigidbody2D>();
        isFlapping = false;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.x <= -1)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, transform.eulerAngles.z);
        }
        else
            transform.rotation = Quaternion.Euler(0f, 180f, transform.eulerAngles.z);
    }

    public void makeFacePlayer()
    {
        float direction = movementObject.transform.position.x - player.transform.position.x;

        if (direction < 0)
            transform.rotation = Quaternion.Euler(0f, 180f, transform.eulerAngles.z);
        else
            transform.rotation = Quaternion.Euler(0f, 0f, transform.eulerAngles.z);
    }

    public void startScreech()
    {
        animator.SetBool("Screeching", true);
    }

    public void endScreech()
    {
        animator.SetBool("Screeching", false);
    }

    public void startSwoop()
    {
        animator.SetBool("Swooping", true);
    }

    public void endSwoop()
    {
        animator.SetBool("Swooping", false);
    }

    public void startDive()
    {
        animator.SetBool("Swooping", true);
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    public void endDive()
    {
        animator.SetBool("Swooping", false);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
