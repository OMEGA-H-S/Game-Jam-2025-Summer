using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip intro;
    [SerializeField] private AudioClip loop;
    [SerializeField] private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(playLoopable());
    }

    private IEnumerator playLoopable()
    {

        yield return new WaitForSeconds(intro.length);
        source.clip = loop;
        source.loop = true;
        source.Play();
    }
}
