using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    [SerializeField] private GameObject displayObj;
    [SerializeField] private string sceneName;
    [SerializeField] private int levelNum = -1;
    private Animator animator;
    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Called once");
        animator = GetComponent<Animator>();
        if(levelNum != -1)
        {
            if(HomeManager.InstanceOfHome.checkCompletion(levelNum) )
            {
                animator.SetTrigger("Complete");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Input.GetKeyDown(KeyCode.Return)) 
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (displayObj != null)
            {
                displayObj.SetActive(true);
                active = true;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (displayObj != null)
            {
                displayObj.SetActive(false);
                active = false;
            }
        }

    }

    
}
