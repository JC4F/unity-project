using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class tmpText : MonoBehaviour
{
    [SerializeField]TMP_Text textTMP;
    public float animationDuration = 1f;

    private bool isAnimating = false;
    private Color startColor;
    private Color endColor;

    private void Start()
    {
        startColor = textTMP.color;

        endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }

    public void StartAnimation()
    {
        if (!isAnimating)
        {
            isAnimating = true;
            StartCoroutine(AlphaAnimationCoroutine());
        }
    }

    //1 -> 0
    private System.Collections.IEnumerator AlphaAnimationCoroutine()
    {
        float currentTime = 0f;
        //Tu 0 frame -> 1s frame
        while (currentTime < animationDuration)
        {
            //Di chuyen do alpha tu tu tu khoang 1f -> 0
            float alpha = Mathf.Lerp(1f, 0f, currentTime / animationDuration);

            Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);

            textTMP.color = newColor;

            //
            currentTime += Time.deltaTime; //Chay tung frame mot nhu animation

            yield return null;
        }

        textTMP.color = endColor;

        isAnimating = false;
        yield return new WaitForSeconds(2f); 
        textTMP.gameObject.SetActive(false);
    }
}
