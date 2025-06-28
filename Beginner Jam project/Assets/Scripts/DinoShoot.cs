using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoShoot : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireCooldown = 1.5f; // how often the dinosaur shoots
    private float fireTimer;

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireCooldown)
        {
            ShootFireball();
            fireTimer = 0f;
        }
    }

    void ShootFireball()
    {
        Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
    }
}
