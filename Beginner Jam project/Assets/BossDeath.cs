using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDeath : MonoBehaviour
{
    // Start is called before the first frame update
    private EnemyHealth boss;
    [SerializeField] private GameObject victoryScreen;
    void Start()
    {
        boss = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(boss.getenemyHealth() <= 0 )
        {
            StartCoroutine(gameOver());
        }
    }

    IEnumerator gameOver()
    {
        victoryScreen.SetActive(true);
        yield return new WaitForSeconds(15);
        SceneManager.LoadScene("Home");
    }
}
