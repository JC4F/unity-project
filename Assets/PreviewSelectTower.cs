using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;
using UnityEngine.UIElements;
using TMPro;

public class PreviewSelectTower : MonoBehaviour
{
    public Sprite hoverSprite;
    public Sprite hoverBanSprite;
    public Sprite currentSprite;
    private Sprite originalSprite;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    public AudioClip hoverSFX;
    public AudioClip towerBuildingSFX;
    public GameObject tower;

    public SpriteRenderer landSprite;
    public GameObject towerSprite;
    //SoliderTower
    public GameObject soliderObjectA;
    public GameObject soliderObjectB;
    //For Military Door Animation
    public GameObject militaryDoorObject;
    //For Arti tower
    public GameObject firePower;
    public GameObject firepowerTowerSprite;
    public GameObject firepowerTowerBuilding;
    //For replaced solider for each tower
    public GameObject replacedSoliderA;
    public GameObject replacedSoliderB;
    //Replaced land building to current build
    public Sprite replacedLand;
    //Sprite current building
    public Sprite landTowerBuilding;
    //Replaced landtowerbuilding to a completed tower
    public Sprite replacedTower;
    //ProgressBar process building and a song when finished
    public GameObject progressBar;
    public Image fillCircle;
    public GameObject cloudEffect;
    //Close list tower build
    public GameObject previewTower;
    public GameObject RangeAttack;
    public GameObject bulletPrefab;
    public TextMeshProUGUI goldValue;

    private void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        audioSource = GetComponent<AudioSource>();
    }

    
    private void OnMouseEnter()
    {

        if (originalSprite == currentSprite)
        {
            spriteRenderer.sprite = hoverSprite;
        }
        else
        {
            spriteRenderer.sprite = hoverBanSprite;
        }
        tower.SetActive(true);
        if (hoverSFX != null)
        {
            audioSource.PlayOneShot(hoverSFX);
        }
    }


    private void OnMouseDown()
    {

        if (originalSprite == currentSprite)
        {
            // Deactivate and start building the tower
            previewTower.GetComponent<PreviewTower>().StartBuildingTower();


            // Set the tag for the tower object
            GameObject clickedObject = gameObject;
            previewTower.tag = clickedObject.tag + "Sentines";

            // Change the sprite and show the progress bar
            landSprite.sprite = replacedLand;
            SpriteRenderer towerSpriteRenderer = towerSprite.GetComponent<SpriteRenderer>();
            towerSpriteRenderer.sprite = landTowerBuilding;
            progressBar.SetActive(true);

            // Play sound effect
            if (towerBuildingSFX != null)
            {
                audioSource.PlayOneShot(towerBuildingSFX);
            }

            // Perform the circle fill over time
            StartCoroutine(FillCircleOverTime());
        }
    }

    private IEnumerator FillCircleOverTime()
    {
        float elapsedTime = 0f;
        float startFillAmount = fillCircle.fillAmount;
        float targetFillAmount = 1f;
        float fillDuration = 0.8f;

        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentFillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / fillDuration);
            fillCircle.fillAmount = currentFillAmount;

            yield return null;
        }

        fillCircle.fillAmount = targetFillAmount;
        TowerBuildComplete();
    }
    private void TowerBuildComplete()
    {
        progressBar.SetActive(false);
        SpriteRenderer towerSpriteRenderer = towerSprite.GetComponent<SpriteRenderer>();
        Animator towerAnimator = towerSprite.GetComponent<Animator>();

        if (replacedTower != null )
        {
            Debug.Log(towerSprite);
            towerSpriteRenderer.sprite = replacedTower;

        }
        else if (replacedTower == null && militaryDoorObject == null)
        {
        }
        else
        {
        }
        previewTower.GetComponent<PreviewTower>().FinishBuildingTower();
        cloudEffect.SetActive(true);
    }
    private void HandleNonFirepowerTower(SpriteRenderer towerSpriteRenderer, Animator towerAnimator)
    {
        towerSpriteRenderer.sprite = replacedTower;
        Vector3 newPosition = new Vector3(535.1f, 254.3f, towerSprite.transform.localPosition.z);
        Quaternion newRotation = Quaternion.Euler(0f, 0f, 90f);
        towerSprite.transform.localPosition = newPosition;
        towerSprite.transform.localRotation = newRotation;

    }
    private void OnMouseExit()
    {
        spriteRenderer.sprite = originalSprite;
        tower.SetActive(false);
    }

    private enum TowerType
    {
        ArrowTower,
        MageTower,
        ArtilleristTower
    }

}