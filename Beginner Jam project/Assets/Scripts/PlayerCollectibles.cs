using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectibles : MonoBehaviour
{
    private List<int> keysCollected;
    // Start is called before the first frame update
    void Start()
    {
        keysCollected = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KeyCollected(int keyCode)
    {
        keysCollected.Add(keyCode);
    }
    public bool hasCode(int keyCode)
    {
        return keysCollected.Contains(keyCode);
    }
}
