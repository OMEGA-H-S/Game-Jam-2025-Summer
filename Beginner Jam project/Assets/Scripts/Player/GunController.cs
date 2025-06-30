using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private Vector3 v1 = Vector3.right;
    protected Transform pivot;
    private Camera camera;
    [SerializeField] private GameObject laser;
    [SerializeField] protected float timeBetweenShot = 0.5f;
    private int offset = 0;

    protected float prevShot;
    private Vector2 comparison = Vector2.right;
    private bool invertLaser;


    private void Awake()
    {
        pivot = transform.parent;
        camera = Camera.main;
        prevShot = 0;
        invertLaser = false;
    }

    protected void Update()
    {
        Rotate();
        prevShot += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && prevShot > timeBetweenShot)
        {
            SpawnLaser();
            prevShot = 0;
        }


    }

    protected void Rotate()
    {
        Vector3 mouseLocation = Input.mousePosition;

        Vector3 location = camera.ScreenToWorldPoint(mouseLocation);
        //location.z = 0;

        float x = location.x - pivot.position.x;
        float y = location.y - pivot.position.y;
        float z = 0;

        Vector3 v2 = new Vector3(x, y, z).normalized;
        //Vector3 v3 = v2 - v1;

        //float gunDirection = getAngle(1, 1, v3.magnitude);

        /*
        if (location.y < pivot.position.y)   //If the mouse is lower than the gun, then the calculations are a bit off
        {
            //gunDirection += 2 * (180 - gunDirection);
            gunDirection *= -1;
        }
        */

        float angle =  Vector2.SignedAngle(comparison, v2);
        //Debug.Log(angle);

        pivot.eulerAngles = new Vector3(0, 0, angle + offset);

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

        



        //Debug.Log("Global: " + pivot.eulerAngles);
        //Debug.Log("Local: " + pivot.localEulerAngles);

        //Debug.Log(Vector2.SignedAngle(Vector2.right, v2));
    }

    /*
    private float getAngle(float a, float b, float c)
    {
        float lawOC = Mathf.Pow(a, 2) + Mathf.Pow(b, 2) - Mathf.Pow(c, 2);
        lawOC /= (2 * a * b);
        return Mathf.Acos(lawOC) * 180 / Mathf.PI;
    }
    */

    protected void SpawnLaser()
    {
        float angle = pivot.eulerAngles.z;
        if(invertLaser)
        {
            angle += 180;
            angle = normalizedAngle(angle);
        }
        Instantiate(laser, transform.position, Quaternion.Euler(new Vector3(0, 0, angle))).GetComponentInChildren<LaserController>().setBulletOwner(LaserController.Owner.Player);
    }

    protected float normalizedAngle(float angle)
    {
        while(angle < -180 || angle > 180)
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

    public void playerSwitchedDirection()
    {
        //Debug.Log("Method called");
        //pivot.transform.localScale = new Vector3(-pivot.localScale.x, -pivot.localScale.y, pivot.localScale.z);

        //offset = (offset == 0) ? 180 : 0;
        //Debug.Log(offset);.
        comparison = comparison == Vector2.right ? Vector2.left : Vector2.right;
        invertLaser = !invertLaser;
    }
}