using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaringWave : MonoBehaviour
{
    public Image circleImage;
    public SpriteRenderer guiIcon;
    public GameObject descriptionPanel;
    public GameObject spawnEnemies;
    public WaveFade waveFade;
    private float fillDuration = 20f;

    public AudioSource audioSource;
    public AudioSource audioSourceClick;
    private bool isDescriptionPanelActive = false;
    private bool isFading = false;
    private Color transparentColor = new Color(1f, 1f, 1f, 0f);
    private GameSystem gameSystem;
    private GameObject[] waveSkipArray;
    private bool hasPlayedAudio1 = false;
    public bool allowToRun;
    private void Start()
    {
        gameSystem = GameObject.FindGameObjectWithTag("GameSystem").GetComponent<GameSystem>();
        if(allowToRun)
        {
            StartCoroutine(FillCircleOverTime());
        }
    }
    public void OnPointerHover()
    {
        if (!isDescriptionPanelActive && !isFading)
        {
            StartCoroutine(FadeInCoroutine());
        }
    }

    public void OnPointerClick()
    {
        if (!isFading)
        {
            if (audioSourceClick != null)
            {
                audioSourceClick.Play();
                gameSystem.IncreaseWave();
                if (audioSource != null && !audioSource.isPlaying)
                {
                    StartCoroutine(PlayDelayedAudioCoroutine());
                    StartCoroutine(SkillCastingOpen());
                }
            }
        }
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

        if (!hasPlayedAudio1)
        {

            PlayAudio();
            hasPlayedAudio1 = true;
        }
    }

    private void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private IEnumerator SkillCastingOpen()
    {
        yield return null;

        GameObject[] skillsLeft = GameObject.FindGameObjectsWithTag("SkillLeft");
        GameObject[] skillsRight = GameObject.FindGameObjectsWithTag("SkillRight");

        foreach (GameObject skillLeft in skillsLeft)
        {
            Animator animator = skillLeft.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("starting");
            }
        }

        foreach (GameObject skillRight in skillsRight)
        {
            Animator animator = skillRight.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("starting");
            }
        }
    }

    private IEnumerator PlayDelayedAudioCoroutine()
    {
        yield return new WaitForSeconds(0.1f);

        if (audioSource != null)
        {
            audioSource.Play();
        }

        StartCoroutine(FadeOutCoroutine(1f));

        int fadeOutCount = 0;
        int totalFadeOutCount = 0;
        waveSkipArray = GameObject.FindGameObjectsWithTag("WaveSkip");
        for (int i = 0; i < waveSkipArray.Length; i++)
        {
            GameObject waveSkipObject = waveSkipArray[i].gameObject;
            StaringWave wave = waveSkipArray[i].GetComponent<StaringWave>();
            if (wave != null && waveSkipObject.name != "TimeSkip")
            {
                gameSystem.CloseSkipWave();
                if (wave.circleImage != null)
                {
                    totalFadeOutCount++;
                    StartCoroutine(FadeOutImageCoroutine(wave.circleImage, () =>
                    {
                        fadeOutCount++;
                        if (fadeOutCount >= totalFadeOutCount)
                        {
                            StartCoroutine(DisableWaveEffectsAfterDelay());
                        }
                    }));
                }

                if (wave.guiIcon != null)
                {
                    totalFadeOutCount++;
                    StartCoroutine(FadeOutSpriteRendererCoroutine(wave.guiIcon, () =>
                    {
                        fadeOutCount++;
                        if (fadeOutCount >= totalFadeOutCount)
                        {
                            StartCoroutine(DisableWaveEffectsAfterDelay());
                        }
                    }));
                }
            }
        }
        spawnEnemies.SetActive(true);
    }

    private IEnumerator DisableWaveEffectsAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        waveSkipArray = GameObject.FindGameObjectsWithTag("WaveSkip");
        for (int i = 0; i < waveSkipArray.Length; i++)
        {
            GameObject waveSkipObject = waveSkipArray[i].gameObject;

            StaringWave wave = waveSkipArray[i].GetComponent<StaringWave>();
            if (wave != null && waveSkipObject.name != "TimeSkip")
            {
                if (wave.circleImage != null)
                {
                    wave.circleImage.transform.parent.parent.gameObject.SetActive(false);
                }

                if (wave.guiIcon != null)
                {
                    wave.guiIcon.transform.parent.parent.gameObject.SetActive(false);
                }
            }
        }
    }

    private IEnumerator FadeOutImageCoroutine(Image image, Action callback)
    {
        float elapsedTime = 0f;
        float fadeTime = 0.5f;
        Color originalColor = image.color;

        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeTime);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = transparentColor;
        callback?.Invoke();
    }

    private IEnumerator FadeOutSpriteRendererCoroutine(SpriteRenderer spriteRenderer, Action callback)
    {
        float elapsedTime = 0f;
        float fadeTime = 0.5f;
        Color originalColor = spriteRenderer.color;

        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeTime);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = transparentColor;
        callback?.Invoke();

    }

    public void HideDescriptionPanel()
    {
        if (isDescriptionPanelActive && !isFading)
        {
            StartCoroutine(FadeOutCoroutine(0.5f));
        }
    }

    private IEnumerator FadeInCoroutine()
    {
        isFading = true;
        descriptionPanel.SetActive(true);
        yield return null;
        isDescriptionPanelActive = true;
        isFading = false;
    }

    private IEnumerator FadeOutCoroutine(float delayTime)
    {
        isFading = true;
        waveFade.StartFadeEffect(false);

        yield return new WaitForSeconds(delayTime);

        descriptionPanel.SetActive(false);
        isDescriptionPanelActive = false;
        isFading = false;
    }
}
