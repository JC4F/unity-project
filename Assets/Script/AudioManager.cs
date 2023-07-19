using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip audioClip;

    private AudioSource audioSource;
    private bool isPlaying = false;

    private void Awake()
    {
        instance = this;
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();
            isPlaying = true;
        }
    }

    public void sceneManager(int sceneNumber)
    {
        StartCoroutine(StartNewGameCoroutine(sceneNumber));
    }

    private IEnumerator StartNewGameCoroutine(int sceneNumber)
    {
        float startVolume = audioSource.volume;
        float t = 0f;
        while (t < 2f)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / 2f);
            yield return null;
        }

        audioSource.volume = 0f;
        yield return new WaitForSeconds(2f);

        if (gameObject != null)
        {
            Destroy(gameObject);
        }

        SceneManager.LoadScene(sceneNumber);
    }
}
