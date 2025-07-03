using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreechMovement : MonoBehaviour
{
    private Vector3 desiredXVector;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        desiredXVector = transform.localPosition + new Vector3(-20f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveTowardsVector = Vector3.MoveTowards(transform.position, desiredXVector, 5f * Time.fixedDeltaTime);
        rb.MovePosition(moveTowardsVector);

        if (moveTowardsVector == transform.position)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
            collision.gameObject.GetComponent<PlayerHealth>().playerAttacked(10);
    }
}
