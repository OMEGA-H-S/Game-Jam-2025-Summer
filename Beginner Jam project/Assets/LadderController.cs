using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    [SerializeField] private Transform top;
    [SerializeField] private Transform bottom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(Vector2.Distance(collision.transform.position, top.transform.position) < 
                Vector2.Distance(collision.transform.position, bottom.transform.position) )
            {
                collision.transform.position = bottom.transform.position;
            }
            else
            {
                collision.transform.position = top.transform.position;
            }
        }
    }
}
