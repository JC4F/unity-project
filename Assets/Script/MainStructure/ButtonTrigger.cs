using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonTrigger : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite downSprite;
    private Sprite originalSprite;
    public AudioSource audioButtonGUI;
    public int sceneNumber;

    private void Start()
    {
        originalSprite = spriteRenderer.sprite;
    }

    public void onPointerDown()
    {
        spriteRenderer.sprite = downSprite;
        audioButtonGUI.Play();
    }

    public void onPointerUp()
    {
        spriteRenderer.sprite = originalSprite;
        PlayerPrefs.SetInt("shouldApplyFade", 0);
        SceneManager.LoadScene(sceneNumber);
    }  
}
