using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveFade : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public TextMeshProUGUI[] textMeshPros;

    private Color spriteStartColor;
    private Color[] textStartColors;
    public Animator[] animators;

    private void Start()
    {
        spriteStartColor = spriteRenderer.color;

        textStartColors = new Color[textMeshPros.Length];
        for (int i = 0; i < textMeshPros.Length; i++)
        {
            textStartColors[i] = textMeshPros[i].color;
        }
    }

    public void StartFadeEffect(bool fadeIn)
    {
        if (fadeIn)
        {
            FadeInCoroutine();
        }
        else
        {
            FadeOutCoroutine();
        }
    }

    private void FadeInCoroutine()
    {

    }

    private void FadeOutCoroutine()
    {
        foreach (Animator animator in animators)
        {
            animator.SetTrigger("FadeOut");
        }
    }
}
