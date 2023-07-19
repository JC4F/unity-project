using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class menuOptionsText : MonoBehaviour
{
    [SerializeField] List<TMP_Text> menuOptions;
    public float animationDuration = 1f;

    private bool isAnimating = false;
    private TMP_ColorGradient startColorGradient;
    private TMP_ColorGradient endColorGradient;

    private void Start()
    {
        foreach (TMP_Text menuOption in menuOptions)
        {
            menuOption.enabled = false;
        }
        startColorGradient = new TMP_ColorGradient();
        startColorGradient.topLeft = Color.clear;
        startColorGradient.topRight = Color.clear;
        startColorGradient.bottomLeft = Color.clear;
        startColorGradient.bottomRight = Color.clear;

        endColorGradient = new TMP_ColorGradient();
        endColorGradient.topLeft = Color.white;
        endColorGradient.topRight = Color.white;
        endColorGradient.bottomLeft = Color.white;
        endColorGradient.bottomRight = Color.white;
    }

    public void ShowMenuOptions()
    {
        if (!isAnimating)
        {
            isAnimating = true;
            StartCoroutine(MenuOptionsAnimationCoroutine(true));
        }
    }

    private IEnumerator MenuOptionsAnimationCoroutine(bool showOptions)
    {

        yield return StartCoroutine(MenuOptionsCoroutine(showOptions));
    }
    public void HideMenuOptions()
    {
        if (!isAnimating)
        {
            isAnimating = true;
            StartCoroutine(MenuOptionsAnimationCoroutine(false));
        }
    }

    private IEnumerator MenuOptionsCoroutine(bool showOptions)
    {
        float currentTime = 0f;
        TMP_ColorGradient targetColorGradient = showOptions ? endColorGradient : startColorGradient;
        bool reachedTargetAlpha = false;

        while (currentTime < animationDuration && !reachedTargetAlpha)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / animationDuration);

            foreach (TMP_Text menuOption in menuOptions)
            {
                if (menuOption.gameObject.name.Equals("loadGame") || menuOption.gameObject.name.Equals("continue"))
                {
                    if (Mathf.Approximately(alpha, 0.72f))
                    {
                        menuOption.colorGradientPreset = targetColorGradient;
                        reachedTargetAlpha = true;
                    }
                    else
                    {
                        TMP_ColorGradient colorGradient = new TMP_ColorGradient(
                            new Color(1f, 1f, 1f, alpha),
                            new Color(1f, 1f, 1f, alpha),
                            new Color(1f, 1f, 1f, alpha),
                            new Color(1f, 1f, 1f, alpha)
                        );
                        menuOption.colorGradientPreset = colorGradient;
                    }
                }
                else
                {
                    menuOption.colorGradientPreset = targetColorGradient;
                }
            }

            currentTime += Time.deltaTime;

            yield return null;
        }

        foreach (TMP_Text menuOption in menuOptions)
        {
            menuOption.colorGradientPreset = endColorGradient;

            if (showOptions)
            {
                menuOption.enabled = true;
            }
            else
            {
                menuOption.enabled = false;
            }
        }

        isAnimating = false;
    }
}
