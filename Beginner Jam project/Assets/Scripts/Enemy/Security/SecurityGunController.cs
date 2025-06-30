using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecurityGunController : MonoBehaviour
{
    private Transform pivot;
    private Transform player;
    private Vector2 comparison;
    private bool invertLaser;

    [SerializeField] private GameObject laser;

    private void Start()
    {
        pivot = transform.parent;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        comparison = Vector2.right;
        invertLaser = true;
        
    }

    private void Update()
    {
        aimAtPlayer();
    }

    private void aimAtPlayer()
    {
        Vector3 direction = player.position - pivot.position;
        //Vector3 v3 = v2 - v1;

        //float gunDirection = getAngle(1, 1, v3.magnitude);

        /*
        if (location.y < pivot.position.y)   //If the mouse is lower than the gun, then the calculations are a bit off
        {
            //gunDirection += 2 * (180 - gunDirection);
            gunDirection *= -1;
        }
        */

        float angle = Vector2.SignedAngle(comparison, direction);
        //Debug.Log("Comparison: " + comparison);
        //Debug.Log(angle);

        pivot.eulerAngles = new Vector3(0, 0, angle);

        //Debug.Log(pivot.localEulerAngles.z);

        //Using normalizedAngle because localEulerAngles sometimes isn't within -180 to 180
        //localEulerAngles is depdendent upon either Vector2.right or Vector2.left, depending on what direction character is facing
        //

        if (normalizedAngle(pivot.localEulerAngles.z) > 70 && normalizedAngle(pivot.localEulerAngles.z) < 110)
        {
            pivot.localEulerAngles = new Vector3(0, 0, 70);
        }
        else if (normalizedAngle(pivot.localEulerAngles.z) >= 110)
        {
            pivot.localEulerAngles = new Vector3(0, 0, 180 - normalizedAngle(pivot.localEulerAngles.z));
        }
        else if (normalizedAngle(pivot.localEulerAngles.z) < -70 && normalizedAngle(pivot.localEulerAngles.z) > -110)
        {
            pivot.localEulerAngles = new Vector3(0, 0, -70);
        }
        else if (normalizedAngle(pivot.localEulerAngles.z) < -110)
        {
            pivot.localEulerAngles = new Vector3(0, 0, -180 - normalizedAngle(pivot.localEulerAngles.z));
        }
    }

    protected float normalizedAngle(float angle)
    {
        while (angle < -180 || angle > 180)
        {
            if (angle > 180)
            {
                return angle -= 360;
            }
            else if (angle < -180)
            {
                return angle += 360;
            }
        }
        return angle;

    }

    public void adjustGunComparison()
    {
        //true represents left, false represnts right
        //Debug.Log("Method called");
        //pivot.transform.localScale = new Vector3(-pivot.localScale.x, -pivot.localScale.y, pivot.localScale.z);

        //offset = (offset == 0) ? 180 : 0;
        //Debug.Log(offset);
        comparison = pivot.position.x > player.position.x ? Vector2.left : Vector2.right;
        //comparison = direction ? Vector2.left : Vector2.right;
        //invertLaser = !invertLaser;
    }

    public void SpawnLaser()
    {
        float angle = pivot.eulerAngles.z;
        if (transform.parent.parent.localScale.x < 0)
        {
            //Debug.Log("Laser is inverted");
            angle += 180;
            angle = normalizedAngle(angle);
        }
        GameObject bullet = Instantiate(laser, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

        bullet.GetComponentInChildren<LaserController>().setBulletOwner(LaserController.Owner.Enemy);
    }

    public void enemySwitchedDirection()
    {
        //Debug.Log("Method called");
        //pivot.transform.localScale = new Vector3(-pivot.localScale.x, -pivot.localScale.y, pivot.localScale.z);

        //offset = (offset == 0) ? 180 : 0;
        //Debug.Log(offset);.
        
        invertLaser = !invertLaser;
    }

}
