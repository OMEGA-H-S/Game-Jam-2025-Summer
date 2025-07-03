using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BinderController : MonoBehaviour
{
    [SerializeField] private int numInDirection = 10;
    private int numGoneInDirection = 0;
    private float increment = 0.05f;
    [SerializeField] private float timeBetweenMovement = 0.01f;
    [SerializeField] private int levelNum;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(moveKey());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Z)) 
        {
            HomeManager.InstanceOfHome.levelCompleted(levelNum);
            Debug.Log("Level done: " + HomeManager.InstanceOfHome.checkCompletion(levelNum));
            SceneManager.LoadScene("Home");
        }
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
        
        if (collision.gameObject.tag == "Player")
        {
            HomeManager.InstanceOfHome.levelCompleted(levelNum);
            Debug.Log("Level done: " + HomeManager.InstanceOfHome.checkCompletion(levelNum));
            SceneManager.LoadScene("Home");
        }

    }
}
