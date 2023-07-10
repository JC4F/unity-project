using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PreviewTower : MonoBehaviour
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
    public Animator TowerPrefabAnimation;
    public Animator TowerPrefabUpgradeAnimation;

    public GameObject[] listTowerAvailable;
    public GameObject[] icon;

    private void Start()
    {
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
                    Debug.Log("isHiding1");

                    isHiding = true;
                    isBuildingTower = true;
                }
                else
                {
                    Debug.Log("isHiding1");

                    isHiding = false;
                    isBuildingTower = false;
                }
            }
            if (isTowerPrefabActive && Input.GetMouseButtonDown(0) && !isMouseClickOnCollider && hasClickedOutsideCollider && !isDeactivating && !hasClickedOnCollider)
            {
                Debug.Log("isClosed1");

                if (isHiding == false)
                {
                    Debug.Log("isClosed2");

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
        }
        else if (TowerPrefabUpgrade != null && !gameObject.CompareTag("Empty"))
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

    private void OnMouseDown()
    {
        Debug.Log("Click");
        Debug.Log(isBuildingTower);
        Debug.Log(isDeactivating);
        if (!isBuildingTower)
        {
            if (!isDeactivating)
            {
                ActivateTowerPrefab();
            }
        }
    }


    private void ActivateTowerPrefab()
    {

        if (gameObject.CompareTag("Empty"))
        {
            TowerPrefab.SetActive(true);
        }
        else
        {
            TowerPrefab.SetActive(true);
            Debug.Log(gameObject.tag);

            GameObject towerObject = Array.Find(listTowerAvailable, element => element.tag + "Sentines" == gameObject.tag) as GameObject;
            towerObject.SetActive(true);
        }

        isTowerPrefabActive = true;
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
