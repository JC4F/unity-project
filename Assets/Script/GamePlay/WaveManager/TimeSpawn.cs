using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimeSpawn : MonoBehaviour
{
    public Image circleImage;
    public GameObject descriptionPanel;
    public WaveFade waveFade;
    private float fillDuration = 30f;

    public AudioSource audioSource;
    private bool hasPlayedAudio = false;
    private bool isDescriptionPanelActive = false;
    private bool isFading = false;

    private void Start()
    {
        StartCoroutine(FillCircleOverTime());
    }

    IEnumerator FillCircleOverTime()
    {
        float elapsedTime = 0f;
        float startFillAmount = 0f;
        float targetFillAmount = 1f;

        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentFillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / fillDuration);
            circleImage.fillAmount = currentFillAmount;

            yield return null;
        }

        circleImage.fillAmount = targetFillAmount;

        yield return new WaitForSeconds(0.1f);

        if (!hasPlayedAudio)
        {

            PlayAudio();
            hasPlayedAudio = true;
        }
    }

    private void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void OnPointerHover()
    {
        if (!isDescriptionPanelActive && !isFading)
        {
            StartCoroutine(FadeInAndActivatePanelCoroutine());
        }
    }

    public void HideDescriptionPanel()
    {
        if (isDescriptionPanelActive && !isFading)
        {
            StartCoroutine(FadeOutAndDeactivatePanelCoroutine());
        }
    }

    private IEnumerator FadeInAndActivatePanelCoroutine()
    {
        isFading = true;
        descriptionPanel.SetActive(true);
        yield return null;
        isDescriptionPanelActive = true;
        isFading = false;
    }

    private IEnumerator FadeOutAndDeactivatePanelCoroutine()
    {
        isFading = true;
        waveFade.StartFadeEffect(false);

        yield return new WaitForSeconds(1f);

        descriptionPanel.SetActive(false);
        isDescriptionPanelActive = false;
        isFading = false;
    }

}