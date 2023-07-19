using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingGame : MonoBehaviour
{
    public GameObject healthGUI;
    public GameObject goldGUI;
    public float startTargetY = 202.94f;
    public float endTargetY = 166.8f;
    public float animationDuration = 1.0f;

    private Vector3 healthGUIStartPosition;
    private Vector3 goldGUIStartPosition;

    private void Start()
    {
        // L?u l?i v? trí ban ??u c?a hai gameObject
        healthGUIStartPosition = healthGUI.transform.position;
        goldGUIStartPosition = goldGUI.transform.position;

        // B?t ??u animation
        StartAnimation();
    }

    private void StartAnimation()
    {
        // T?o coroutine ?? th?c hi?n animation t? v? trí ban ??u ??n v? trí m?i
        StartCoroutine(AnimateObjects());
    }

    private System.Collections.IEnumerator AnimateObjects()
    {
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            // Tính toán t?a ?? Y m?i c?a các gameObject d?a trên th?i gian ?ã trôi qua và th?i gian animation
            float t = elapsedTime / animationDuration;
            float newY1 = Mathf.Lerp(healthGUIStartPosition.y, endTargetY, t);
            float newY2 = Mathf.Lerp(goldGUIStartPosition.y, endTargetY, t);

            // C?p nh?t v? trí m?i cho các gameObject
            Vector3 newPosition1 = new Vector3(healthGUI.transform.position.x, newY1, healthGUI.transform.position.z);
            Vector3 newPosition2 = new Vector3(goldGUI.transform.position.x, newY2, goldGUI.transform.position.z);
            healthGUI.transform.position = newPosition1;
            goldGUI.transform.position = newPosition2;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ??m b?o ??t v? trí cu?i cùng c?a các gameObject thành v? trí m?i
        healthGUI.transform.position = new Vector3(healthGUI.transform.position.x, endTargetY, healthGUI.transform.position.z);
        goldGUI.transform.position = new Vector3(goldGUI.transform.position.x, endTargetY, goldGUI.transform.position.z);
    }
}
