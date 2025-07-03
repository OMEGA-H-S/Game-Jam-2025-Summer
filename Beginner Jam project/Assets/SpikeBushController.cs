using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeBushController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float gravityScale = 30f;
    [SerializeField] private float timer = 0;
    [SerializeField] private float hurtTimer = 1f;
    [SerializeField] private float drag = 50f;
    [SerializeField] private float playerDamage = 5f;

    [SerializeField] private AudioClip bushSound;

    private bool inBush;
    private void Start()
    {
        inBush = false;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            inBush = true;
            StartCoroutine(BushHit());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
            StopCoroutine(BushHit());
        inBush = false;

        // Reset Speed
        player.GetComponent<Rigidbody2D>().gravityScale = 7.87f;
        player.GetComponent<Rigidbody2D>().drag = 0;
    }

    IEnumerator BushHit()
    {
        while (inBush) // While in bush
        {
            // Make it so the player is slower
            player.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
            player.GetComponent<Rigidbody2D>().drag = drag;

            // Run a timer to make the player take ticks of damage
            if (timer <= 0)
            {
                SoundEffectsManager.instance.PlaySoundEffectClip(bushSound, transform, 0.3f);
                player.GetComponent<PlayerHealth>().playerAttacked(playerDamage);
                timer = hurtTimer;
            } else
            {
                timer -= Time.deltaTime;
            }

            yield return null;
        }
    }
}
