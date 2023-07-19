using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class textCursor : MonoBehaviour
{
    [SerializeField] private TMP_Text textMeshPro;
    [SerializeField] private List<Animator> borderBackgroundAnimators;
    [SerializeField] private Animator borderBackgroundAnimator;
    [SerializeField] AudioSource audioSource;

    public void OnPointerEnter()
    {
        foreach (var animator in borderBackgroundAnimators)
        {
            animator.gameObject.SetActive(false);
        }
        CheckVertexColorAlpha();

    }

    public void OnPointerExit()
    {
        borderBackgroundAnimator.gameObject.SetActive(false);
    }

    private void CheckVertexColorAlpha()
    {
        TMP_MeshInfo[] meshInfo = textMeshPro.textInfo.meshInfo;
        bool hasAlpha72 = false;
        bool hasAlpha255 = false;

        for (int i = 0; i < meshInfo.Length; i++)
        {
            Color32[] vertexColors = meshInfo[i].colors32;

            for (int j = 0; j < vertexColors.Length; j++)
            {
                float alpha = vertexColors[j].a / 255f;

                if (Mathf.Abs(alpha - 0.28235294f) <= 0.01f)
                {
                    hasAlpha72 = true;
                }
                else if (Mathf.Approximately(alpha, 1f))
                {
                    hasAlpha255 = true;
                }
            }
        }

        if (hasAlpha72)
        {
        }
        else if (hasAlpha255)
        {
            audioSource.Play();
            borderBackgroundAnimator.gameObject.SetActive(true);
        }
    }
}
