using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoShoot : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireCooldown = 0.5f;
    private float fireTimer;
    private SpriteRenderer spriteRenderer;
    private Movement wander;
    private bool hasAlreadyFiredSinceSeeingPlayer = false;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        wander = GetComponent<Movement>();
    }

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (wander.hasLineOfSight)
        {
            Debug.Log("hasLineOfSight = true in DinoShoot");
            if (!hasAlreadyFiredSinceSeeingPlayer)
            {
                // Shoot immediately on first detection
                ShootFireball();
                fireTimer = 0f;
                hasAlreadyFiredSinceSeeingPlayer = true;
            }
            else if (fireTimer >= fireCooldown)
            {
                // Shoot again if cooldown elapsed
                ShootFireball();
                fireTimer = 0f;
            }
        }
        else
        {
            // Reset flag when player goes out of sight
            hasAlreadyFiredSinceSeeingPlayer = false;
        }

    }

    void ShootFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        Fireball fb = fireball.GetComponent<Fireball>();

        // Shoot left if flipped, otherwise right
        //Vector2 direction = spriteRenderer.transform ? Vector2.left : Vector2.right;
        fb.Launch(transform.localScale.x < 0 ? Vector2.left : Vector2.right);
    }


}