using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedSong : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        StartCoroutine(PlayDelayedAudio());
    }

    private System.Collections.IEnumerator PlayDelayedAudio()
    {
        yield return new WaitForSeconds(1.5f);

        audioSource.Play();
    }
}
