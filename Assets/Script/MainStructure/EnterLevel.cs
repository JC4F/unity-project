using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterLevel : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite downSprite;
    private Sprite originalSprite;
    public AudioSource audioButtonGUI;
    public AudioClip audioEnterLevel;
    public SpriteRenderer spriteBG;
    public AudioManager audioManager;
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
        audioButtonGUI.PlayOneShot(audioEnterLevel);
        spriteBG.gameObject.SetActive(true);
        PlayerPrefs.SetInt("shouldApplyFade", 1);
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.sceneManager(sceneNumber);
    }
}
