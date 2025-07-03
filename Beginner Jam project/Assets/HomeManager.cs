using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HomeManager : MonoBehaviour
{
    private static int numLevels;
    [SerializeField] private static bool[] completeLevels;

    public static HomeManager InstanceOfHome;
    [SerializeField] private GameObject bossPortal;
    private bool bossComplete;
    // Start is called before the first frame update

    private void Awake()
    {
        if(InstanceOfHome == null)
        {
            InstanceOfHome = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        numLevels = 4;
        completeLevels = new bool[numLevels];
        for(int i = 0; i < completeLevels.Length; i++)
        {
            //Debug.Log("Resseting");
            completeLevels[i] = false;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public bool checkCompletion(int level)
    {
        if (completeLevels[level - 1])
        {
            //If the level is completed
            return true;
        }
        return false;
    }

    public void levelCompleted(int level)
    {
        completeLevels[level - 1] = true;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Home")
        {
            bool bossBattle = true;
            for (int i = 0; i < completeLevels.Length - 1; i++)
            {
                if (!completeLevels[i])
                {
                    bossBattle = false;
                }
            }
            bossPortal = GameObject.FindGameObjectWithTag("Boss Portal");
            if (bossBattle)
            {
                
                Debug.Log(bossPortal);
                bossPortal.SetActive(true);
            }
            else
            {
                bossPortal.SetActive(false);
            }
        }
        
    }




}
