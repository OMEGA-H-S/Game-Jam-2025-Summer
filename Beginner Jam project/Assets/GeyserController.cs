using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserController : MonoBehaviour
{
    [SerializeField] private float geyserForce = 150f;
    [SerializeField] private AudioClip geyser;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            SoundEffectsManager.instance.PlaySoundEffectClip(geyser, transform, 0.3f);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, geyserForce), ForceMode2D.Impulse);
        }
    }
}
