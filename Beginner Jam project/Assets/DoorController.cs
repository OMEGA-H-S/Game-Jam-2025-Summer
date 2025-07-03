using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private int doorCode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            PlayerCollectibles player = collision.gameObject.GetComponent<PlayerCollectibles>();
            if(player.hasCode(this.doorCode))
            {
                this.openDoor();
            }
        }
    }

    private void openDoor()
    {
        GameObject.Destroy(this.gameObject);
    }
}
