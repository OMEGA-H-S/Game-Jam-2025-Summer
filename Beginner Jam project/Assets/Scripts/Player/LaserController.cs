using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] private float speed;   //Should be very high
    [SerializeField] private float lifetime;
    private Vector2 startPos;
    public enum Owner
    {
        Player, 
        Enemy
    };
    private Owner bulletOwner;

    private float timeInAir;


    private void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x + speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
        

        if (Mathf.Abs(transform.localPosition.x) > 20)
        {
            Destroy(transform.parent.gameObject);
        }

    }

    public void setBulletOwner(Owner val)
    {
        this.bulletOwner = val;
    }

    public Owner getOwner()
    {
        return this.bulletOwner;
    }
}
