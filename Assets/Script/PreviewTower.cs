using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PreviewTower : MonoBehaviour
{
    public GameObject TowerPrefab;
    public GameObject TowerPrefabUpgrade;




    private void Start()
    {
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                ActivateTowerPrefab();

                GameObject clickedObject = hit.collider.gameObject;
                string clickedObjectTag = clickedObject.tag;
            }
            else
            {
                TowerPrefab.SetActive(false);

            }
        }

    }





    private void ActivateTowerPrefab()
    {
            TowerPrefab.SetActive(true);
    }


}