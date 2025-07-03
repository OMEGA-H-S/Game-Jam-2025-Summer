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
            Instantiate(screechObject, new Vector3(GetComponentInParent<Transform>().position.x, yFloatLevel + 2.3f), GetComponentInParent<Transform>().rotation);
        } else
        {
            Instantiate(screechObject, new Vector3(GetComponentInParent<Transform>().position.x, yFloatLevel - 1f), GetComponentInParent<Transform>().rotation);
        }
    }
}
