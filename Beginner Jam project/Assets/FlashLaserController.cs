using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLaserController : MonoBehaviour
{
    private LazerMazeController laser;
    // Start is called before the first frame update
    void Start()
    {
        laser = transform.parent.gameObject.GetComponent<LazerMazeController>();
    }

    public void clear()
    {
        laser.clearLaserTriggers();
    }

    public void activate()
    {
        laser.activateLaserTriggers();
    }
}
