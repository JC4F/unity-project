using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;
using UnityEngine.UIElements;
using TMPro;

public class PreviewSelectTower : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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

    private enum TowerType
    {
        ArrowTower,
        MageTower,
        ArtilleristTower
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (originalSprite == currentSprite)
        {
            StartCoroutine(previewTower.GetComponent<PreviewTower>().DeactivateTowerPrefabWithDelay(1f));
            // Deactivate and start building the tower
            previewTower.GetComponent<PreviewTower>().StartBuildingTower();
            // Get a reference to the GameSystem from the GameObject with the tag "GameSystem"
            GameSystem gameSystem = GameObject.FindGameObjectWithTag("GameSystem").GetComponent<GameSystem>();

            // Deduct the gold value
            int goldAmount = int.Parse(goldValue.text);
            gameSystem.SpendGold(goldAmount);

            // Set the tag for the tower object
            GameObject clickedObject = eventData.pointerPress;
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

        if (replacedTower != null && militaryDoorObject == null)
        {
            HandleNonFirepowerTower(towerSpriteRenderer, towerAnimator);
        }
        else if (replacedTower == null && militaryDoorObject == null)
        {
            HandleFirepowerTower(towerSpriteRenderer, towerAnimator);
        }
        else
        {
            HandleMilitaryTower(towerSpriteRenderer, towerAnimator);
        }
        RangeAttack.GetComponent<PolygonCollider2D>().enabled = true;
        previewTower.GetComponent<PreviewTower>().FinishBuildingTower();
        cloudEffect.SetActive(true);
        fillCircle.fillAmount = 0;
    }

    private void HandleNonFirepowerTower(SpriteRenderer towerSpriteRenderer, Animator towerAnimator)
    {
        towerSpriteRenderer.sprite = replacedTower;
        Vector3 newPosition = new Vector3(535.1f, 254.3f, towerSprite.transform.localPosition.z);
        Quaternion newRotation = Quaternion.Euler(0f, 0f, 90f);
        towerSprite.transform.localPosition = newPosition;
        towerSprite.transform.localRotation = newRotation;

        SpriteRenderer soliderA = soliderObjectA.GetComponent<SpriteRenderer>();
        Animator Ani_soliderA = soliderObjectA.GetComponent<Animator>();

        DestroyAllComponents(soliderObjectA);

        //Arrow Tower
        if (replacedSoliderA != null && replacedSoliderB != null)
        {
            DestroyAllComponents(soliderObjectB);
            SpriteRenderer soliderB = soliderObjectB.GetComponent<SpriteRenderer>();
            Animator Ani_soliderB = soliderObjectB.GetComponent<Animator>();
            //Modifier Tower Information
            HandleSolider(soliderA, Ani_soliderA, 534.25f, 269f, soliderA.transform.localPosition.z - 1.0f, 0f, replacedSoliderA);
            HandleSolider(soliderB, Ani_soliderB, 545.88f, 269f, soliderB.transform.localPosition.z - 1.0f, 0f, replacedSoliderB);
            SetRangeAttackScale(6.3f, 4.04f);
            AddTowerComponent(soliderObjectA, TowerType.ArrowTower, 0, 300);
            AddTowerComponent(soliderObjectB, TowerType.ArrowTower, 0, 300);
        }
        //Mage Tower
        else if (replacedSoliderA != null && replacedSoliderB == null)
        {
            //Modifier Tower Information
            HandleSolider(soliderA, Ani_soliderA, 538.45f, 270.19f, soliderA.transform.localPosition.z - 1.0f, 0f, replacedSoliderA);
            SetRangeAttackScale(6.3f, 4.04f);
            AddTowerComponent(soliderObjectA, TowerType.MageTower, 0, 240);
        }
    }
    private void SetRangeAttackScale(float xScale, float yScale)
    {
        Vector3 rangeAttackScale = new Vector3(xScale, yScale, RangeAttack.transform.localScale.z);
        RangeAttack.transform.localScale = rangeAttackScale;
    }

    private void AddTowerComponent(GameObject gameObject, TowerType towerType, int arcHeight, int speed)
    {
        switch (towerType)
        {
            case TowerType.ArrowTower:
                var arrowTower = gameObject.AddComponent<ArrowTower>();
                arrowTower.polygonCollider = RangeAttack.GetComponent<PolygonCollider2D>();
                arrowTower.arcHeight = arcHeight;
                arrowTower.speed = speed;
                arrowTower.bulletPrefab = bulletPrefab;
                RangeAttack.GetComponent<TowerDetected>().SoliderA = soliderObjectA;
                RangeAttack.GetComponent<TowerDetected>().SoliderB = soliderObjectB;
                break;
            case TowerType.MageTower:
                var mageTower = gameObject.AddComponent<MageTower>();
                mageTower.polygonCollider = RangeAttack.GetComponent<PolygonCollider2D>();
                mageTower.arcHeight = arcHeight;
                mageTower.speed = speed;
                mageTower.bulletPrefab = bulletPrefab;
                RangeAttack.GetComponent<TowerDetected>().SoliderA = soliderObjectA;
                break;
            case TowerType.ArtilleristTower:
                var firePower = gameObject.AddComponent<ArtilleristTower>();
                firePower.polygonCollider = RangeAttack.GetComponent<PolygonCollider2D>();
                firePower.arcHeight = arcHeight;
                firePower.speed = speed;
                firePower.bulletPrefab = bulletPrefab;
                RangeAttack.GetComponent<TowerDetected>().SoliderA = soliderObjectA;
                RangeAttack.GetComponent<TowerDetected>().SoliderB = soliderObjectB;
                RangeAttack.GetComponent<TowerDetected>().BarrageFirePower = gameObject;
                break;
        }

    }

    private void DestroyAllComponents(GameObject gameObject)
    {
        Component[] components = gameObject.GetComponents<MonoBehaviour>();
        foreach (var component in components)
        {
            Destroy(component);
        }
    }

    private void HandleSolider(SpriteRenderer solider, Animator Ani_solider, float xPos, float yPos, float zPos, float zRotate, GameObject replacedSolider)
    {
        Vector3 soliderPosition = new Vector3(xPos, yPos, zPos);
        Quaternion soliderRotate = Quaternion.Euler(0f, 0f, zRotate);
        solider.transform.localPosition = soliderPosition;
        solider.transform.localRotation = soliderRotate;

        solider.sprite = replacedSolider.GetComponent<SpriteRenderer>().sprite;
        Ani_solider.runtimeAnimatorController = replacedSolider.GetComponent<Animator>().runtimeAnimatorController;
    }

    private void HandleFirepowerTower(SpriteRenderer towerSpriteRenderer, Animator towerAnimator)
    {
        SpriteRenderer firepowerSpriteRenderer = firepowerTowerSprite.GetComponent<SpriteRenderer>();
        Animator firepowerAnimator = firepowerTowerSprite.GetComponent<Animator>();

        towerSpriteRenderer.sprite = firepowerSpriteRenderer.sprite;
        towerAnimator.runtimeAnimatorController = firepowerAnimator.runtimeAnimatorController;

        Vector3 newPosition = new Vector3(537f, 251.4f, towerSprite.transform.localPosition.z);
        towerSprite.transform.localPosition = newPosition;

        firePower.SetActive(true);

        HandleSolider(soliderObjectA.GetComponent<SpriteRenderer>(), soliderObjectA.GetComponent<Animator>(), 559.2f, 253.2f, soliderObjectA.transform.localPosition.z - 1f, 0f, replacedSoliderA);
        HandleSolider(soliderObjectB.GetComponent<SpriteRenderer>(), soliderObjectB.GetComponent<Animator>(), 515.5f, 257.5f, soliderObjectB.transform.localPosition.z, 0f, replacedSoliderB);

        towerAnimator.enabled = false;
        firepowerAnimator.enabled = false;
        firepowerTowerBuilding.SetActive(true);
        SetRangeAttackScale(7.03f, 4.04f);
        AddTowerComponent(firepowerTowerBuilding, TowerType.ArtilleristTower, 60, 80);
    }

    private void HandleMilitaryTower(SpriteRenderer towerSpriteRenderer, Animator towerAnimator)
    {
        towerSpriteRenderer.sprite = replacedTower;
        Vector3 newPosition = new Vector3(534f, 256f, towerSpriteRenderer.transform.localPosition.z);
        Quaternion newRotation = Quaternion.Euler(0f, 0f, 90f);
        towerSprite.transform.localPosition = newPosition;
        towerSprite.transform.localRotation = newRotation;
        militaryDoorObject.SetActive(true);
        //Modifier Tower Information
        SetRangeAttackScale(6.46f, 4.04f);
    }

    public void OnPointerEnter(PointerEventData eventData)
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

    public void OnPointerExit(PointerEventData eventData)
    {
        spriteRenderer.sprite = originalSprite;
        tower.SetActive(false);
    }
}