using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreechController : MonoBehaviour
{
    [SerializeField] GameObject screechObject;

    public void SpawnObject(bool isUp, float yFloatLevel)
    {
        if (isUp)
        {
            Instantiate(screechObject, new Vector3(GetComponentInParent<Transform>().position.x, yFloatLevel - 0.9f), GetComponentInParent<Transform>().rotation);
        } else
        {
            Debug.Log(yFloatLevel - 2.8);
            Instantiate(screechObject, new Vector3(GetComponentInParent<Transform>().position.x, yFloatLevel - 6f), GetComponentInParent<Transform>().rotation);
        }
    }
}
