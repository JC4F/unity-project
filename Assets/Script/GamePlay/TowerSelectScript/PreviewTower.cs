using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PreviewTower : MonoBehaviour, IPointerClickHandler
{
    public GameObject TowerPrefab;
    public GameObject TowerPrefabUpgrade;

    private bool isTowerPrefabActive = false;
    private bool isBuildingTower = false;
    private bool isMouseClickOnCollider = false;
    private bool hasClickedOutsideCollider = false;
    private bool isDeactivating = false;
    private bool hasClickedOnCollider = false;
    private bool isHiding = false;

    public AudioClip clickSound;
    private AudioSource audioSource;

    public Animator TowerPrefabAnimation;
    public Animator TowerPrefabUpgradeAnimation;

    public GameObject[] listTowerAvailable;
    public GameObject[] icon;
    public TextMeshProUGUI[] costText;

    public Sprite ArrowReplaced;
    public Sprite ArtilleristReplaced;
    public Sprite MageReplaced;
    public Sprite MilitaryReplaced;

    public Sprite ArrowSprite;
    public Sprite ArtillerisSprite;
    public Sprite MageSprite;
    public Sprite MilitarySprite;

    private GameSystem gameSystem;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameSystem = GameObject.FindGameObjectWithTag("GameSystem").GetComponent<GameSystem>();
        Sprite arrowSprite = Array.Find(icon, element => element.tag == "Arrow")?.GetComponent<SpriteRenderer>().sprite;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                string clickedObjectTag = clickedObject.tag;

                if (clickedObjectTag == "Arrow" || clickedObjectTag == "Mage" || clickedObjectTag == "Military" || clickedObjectTag == "Artillerist")
                {
                    isHiding = true;
                    isBuildingTower = true;
                }
                else
                {
                    isHiding = false;
                    isBuildingTower = false;
                }
            }
        }

        if (isTowerPrefabActive && Input.GetMouseButtonDown(0) && !isMouseClickOnCollider && hasClickedOutsideCollider && !isDeactivating && !hasClickedOnCollider)
        {
            if(isHiding == false)
            {
                StartCoroutine(DeactivateTowerPrefabWithDelay(isBuildingTower ? 1f : 0.45f));
                hasClickedOutsideCollider = false;
            }
        }

        isMouseClickOnCollider = false;
        hasClickedOnCollider = false;
        if (Input.GetMouseButtonDown(0) && !IsMouseOverTowerPrefab())
        {
            hasClickedOutsideCollider = true;
        }
    }

    private void updatePriceTower()
    {
        for (int i = 0; i < icon.Length; i++)
        {
            int cost = int.Parse(costText[i].text);
            string towerTag = icon[i].tag;

            if (icon[i].CompareTag(costText[i].tag))
            {
                if (gameSystem.goldValue < cost)
                {
                    Sprite replacedSprite = GetReplacedSprite(towerTag);
                    icon[i].GetComponent<SpriteRenderer>().sprite = replacedSprite;
                    costText[i].color = Color.gray;
                }
                else
                {
                    Sprite defaultSprite = GetDefaultSprite(towerTag);
                    icon[i].GetComponent<SpriteRenderer>().sprite = defaultSprite;
                    costText[i].color = Color.yellow;
                }
            }
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isBuildingTower)
        {
            if (!isDeactivating)
            {
                ActivateTowerPrefab();
            }
            isMouseClickOnCollider = true;
            hasClickedOnCollider = true;
        }
    }

    private bool IsMouseOverTowerPrefab()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = Physics2D.OverlapPoint(mousePosition);
        if (collider != null && (collider.gameObject == TowerPrefab || collider.gameObject == TowerPrefabUpgrade))
        {
            return true;
        }
        return false;
    }

    private void ActivateTowerPrefab()
    {
        if (gameObject.CompareTag("Empty"))
        {
            updatePriceTower();
            TowerPrefab.SetActive(true);
        }
        else
        {
            TowerPrefabUpgrade.SetActive(true);
            GameObject towerObject = Array.Find(listTowerAvailable, element => element.tag + "Sentines" == gameObject.tag) as GameObject;
            towerObject.SetActive(true);
        }

        isTowerPrefabActive = true;

        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    private Sprite GetReplacedSprite(string towerTag)
    {
        switch (towerTag)
        {
            case "Arrow":
                return ArrowReplaced;
            case "Artillerist":
                return ArtilleristReplaced;
            case "Mage":
                return MageReplaced;
            case "Military":
                return MilitaryReplaced;
            default:
                return null;
        }
    }

    private Sprite GetDefaultSprite(string towerTag)
    {
        switch (towerTag)
        {
            case "Arrow":
                return ArrowSprite;
            case "Artillerist":
                return ArtillerisSprite;
            case "Mage":
                return MageSprite;
            case "Military":
                return MilitarySprite;
            default:
                return null;
        }
    }

    public IEnumerator DeactivateTowerPrefabWithDelay(float delay)
    {
        isDeactivating = true;

        if (TowerPrefab != null && gameObject.CompareTag("Empty"))
        {
            TowerPrefabAnimation.SetBool("isClosed", true);
            yield return new WaitForSeconds(delay);

            if (TowerPrefab.activeSelf)
            {
                TowerPrefab.SetActive(false);
            }
        }else if (TowerPrefabUpgrade != null && !gameObject.CompareTag("Empty"))
        {
            TowerPrefabUpgradeAnimation.SetBool("isClosed", true);
            yield return new WaitForSeconds(delay);

            if (TowerPrefabUpgrade.activeSelf)
            {
                TowerPrefabUpgrade.SetActive(false);
            }
        }

        foreach (GameObject tower in listTowerAvailable)
        {
            if (tower.activeSelf)
            {
                tower.SetActive(false);
            }
        }

        isTowerPrefabActive = false;
        isDeactivating = false;
    }

    public void StartBuildingTower()
    {
        isBuildingTower = true;
    }

    public void FinishBuildingTower()
    {
        isBuildingTower = false;
    }
}