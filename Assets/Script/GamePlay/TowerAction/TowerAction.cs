using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerAction : MonoBehaviour, IPointerClickHandler
{
    public GameObject objectTower;
    public GameObject landTower;
    public GameObject polygonRangeTower;
    public GameObject cloudEffect;

    public GameObject[] Soliders;
    public GameObject[] SubdionaryTower;

    private GameSystem gameSystem;

    public Sprite landTowerDefault;

    //For Firepower only
    public GameObject firepowerEnhanced;
    void Start()
    {
        gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //Set a animation for upgradeTowerList
        PreviewTower previewTower = landTower.GetComponent<PreviewTower>();
        StartCoroutine(previewTower.DeactivateTowerPrefabWithDelay(1f));
        if (tag.Equals("Arrow"))
        {
            OnActionTowerSell(70);
        }
        else if (tag.Equals("Artillerist"))
        {
            OnActionTowerBarrageEnhanced(125);
        }
        else if (tag.Equals("Mage"))
        {
            OnActionTowerSell(110);
        }else if (tag.Equals("Military"))
        {

        }
    }

    private void OnActionTowerSell(int goldEarn)
    {
        Vector3 newPosition = new Vector3(538f, 249.5f, objectTower.transform.localPosition.z);
        objectTower.transform.localPosition = newPosition;

        //GameObject -> SpriteRendered -> Sprite = null -> Sprite
        //GetComponent<SpriteRendered>().sprite = null
        //GameObject -> Animator -> runTimeAnimationController -> Controller (sai) = null
        //objectTower dang co Rotation z là 0 -> 90 -> 0 Quaternion.Euler
        //Set Default for objecTower
        objectTower.GetComponent<SpriteRenderer>().sprite = null;
        objectTower.GetComponent<Animator>().runtimeAnimatorController = null;
        objectTower.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 0f);
        //Set default for landTower
        landTower.GetComponent<SpriteRenderer>().sprite = landTowerDefault;

        landTower.tag = "Empty"; //Gan tag thanh cong -> Tag có tên là empty ph?i t?n t?i trong danh sách tag
        //Set default for solider
        foreach (GameObject solider in Soliders)
        {
            if (solider != null)
            {
                solider.GetComponent<Animator>().runtimeAnimatorController = null;
                solider.GetComponent<SpriteRenderer>().sprite = null;
                if (solider.GetComponent<ArrowTower>() != null)
                {
                    ArrowTower arrowTower = solider.GetComponent<ArrowTower>();
                    Destroy(arrowTower);
                }
            }
            else
            {
                break;
            }
        }

        //Set default for rangeTower
        polygonRangeTower.GetComponent<PolygonCollider2D>().enabled = false;
        polygonRangeTower.GetComponent<TowerDetected>().BarrageFirePower = null;
        cloudEffect.SetActive(true);
        gameObject.GetComponent<AudioSource>().Play();
        gameSystem.EarnGold(goldEarn);
    }

    private void OnActionTowerBarrageEnhanced(int goldEarn)
    {
        Vector3 newPosition = new Vector3(538f, 249.5f, objectTower.transform.localPosition.z);
        objectTower.transform.localPosition = newPosition;

        //Set Default for objecTower
        objectTower.GetComponent<SpriteRenderer>().sprite = null;
        objectTower.GetComponent<Animator>().runtimeAnimatorController = null;
        objectTower.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 0f);
        //Set default for landTower
        landTower.GetComponent<SpriteRenderer>().sprite = landTowerDefault;

        landTower.tag = "Empty";
        //Set default for solider
        foreach (GameObject solider in Soliders)
        {
            if (solider != null)
            {
                solider.GetComponent<Animator>().runtimeAnimatorController = null;
                solider.GetComponent<SpriteRenderer>().sprite = null;
            }
            else
            {
                break;
            }
        }

        //Remove Artiellerist FileScript
        Transform firePowerChild = firepowerEnhanced.transform.Find("firePowerEnhanced");
        ArtilleristTower artilleristFileScript = firePowerChild.GetComponent<ArtilleristTower>();
        Destroy(artilleristFileScript);
        firepowerEnhanced.SetActive(false);
        //Set default for rangeTower
        polygonRangeTower.GetComponent<PolygonCollider2D>().enabled = false;
        polygonRangeTower.GetComponent<TowerDetected>().BarrageFirePower = null;
        cloudEffect.SetActive(true);
        gameObject.GetComponent<AudioSource>().Play();
        gameSystem.EarnGold(goldEarn);
    }
}
