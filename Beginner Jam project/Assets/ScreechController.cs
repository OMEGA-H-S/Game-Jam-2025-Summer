using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreechController : MonoBehaviour
{
    [SerializeField] GameObject screechObject;

    public void SpawnObject()
    {
        Instantiate(screechObject, GetComponentInParent<Transform>().position, new Quaternion());
    }
}
