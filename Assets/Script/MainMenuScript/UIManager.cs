using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] AudioSource finalChoiceSound;
    [SerializeField] AudioSource optionChoiceSound;
    [SerializeField] public RawImage rawImage;
    public float fadeOutDuration = 2f;


    public void OnNewGameClick()
    {
        finalChoiceSound.Play();
        rawImage.gameObject.SetActive(true);

        StartCoroutine(StartNewGameCoroutine());
    }

    private IEnumerator StartNewGameCoroutine()
    {
        float startVolume = backgroundMusic.volume;
        float t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            backgroundMusic.volume = Mathf.Lerp(startVolume, 0f, t / fadeOutDuration);
            yield return null;
        }

        backgroundMusic.volume = 0f;
        yield return new WaitForSeconds(4f);
        PlayerPrefs.SetInt("shouldApplyFade", 1);

        SceneManager.LoadScene(1);
    }

    public void OnContinueClick()
    {
        //Code mo khi nhan lua chon Continue

    }

    public void OnLoadGameClick()
    {
        //Code mo khi nhan lua chon LoadGame

    }

    public void OnOptionsClick()
    {
        //Code mo khi nhan lua chon options
    }
}
