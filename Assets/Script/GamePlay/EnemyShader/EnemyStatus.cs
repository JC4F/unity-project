using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnemyStatus : MonoBehaviour
{
  public GameObject bloodEffect;
  public GameObject HPBarrageLeft;
  public int maxHealth;
  public int health;
  public GameObject HPBarrage;

  public int minPhysicalDamage;
  public int maxPhysicalDamage;

  public float attackRate;

  public int armorRating;
  public int magicResistance;

  public int livesTaken;
  public int bounty;

  private Animator animator;
  private Image HPBarrageFill;
  private PathScript pathScript;
  private Animator animatorBloodEffect;
  private GameSystem gameSystem;
  private void Start()
  {
    health = maxHealth;
    animator = GetComponent<Animator>();
    pathScript = GetComponent<PathScript>();
    animatorBloodEffect = bloodEffect.GetComponent<Animator>();

    HPBarrageFill = HPBarrage.GetComponent<Image>();
    HPBarrageFill.fillAmount = 1f;
    gameSystem = GameObject.FindGameObjectWithTag("GameSystem").GetComponent<GameSystem>();
  }

  public void TakeDamage(int damage, string typeDamage, string towerName, GameObject target, float multipleDamage)
  {

    if (towerName == "Arrow")
    {
      animatorBloodEffect.SetTrigger("arrow");
    }
    else if (towerName == "Artillerist")
    {
      if (target == gameObject && multipleDamage == 1)
      {
        createSavePoint(target);
      }
    }
    else if (towerName == "Mage")
    {
      animatorBloodEffect.SetTrigger("mage");
    }
    if (typeDamage == "Physical")
    {
      int effectivePhysicalDamage = Mathf.Max((int)(damage * (1 - (armorRating / (armorRating + 100)))), 0);
      effectivePhysicalDamage -= armorRating;
      effectivePhysicalDamage = Mathf.Max(effectivePhysicalDamage, 0);
      health -= effectivePhysicalDamage;
    }
    else if (typeDamage == "Magic")
    {
      float magicDamageReductionPercentage = (float)magicResistance / (magicResistance + 100);
      damage = Mathf.RoundToInt(damage * (1 - magicDamageReductionPercentage));
      health -= damage;
    }

    health = Mathf.Max(health, 0);

    HPBarrageFill.fillAmount = (float)health / maxHealth;

    if (health <= 0)
    {
      gameSystem.EnemiesKilled();
      gameSystem.EarnGold(bounty);
      gameObject.tag = "Inactive";
      HPBarrageLeft.SetActive(false);
      pathScript.moveSpeed = 0f;
      animator.SetTrigger("isDead");
      Destroy(gameObject, 2f);
    }
  }

  private void createSavePoint(GameObject target)
  {
    GameObject savePoint = GameObject.Find("Canvas/Manager/Firepower/HiddenProjectile");
    GameObject hippePoint = Instantiate(bloodEffect, target.transform.position, Quaternion.identity);

    hippePoint.transform.SetParent(savePoint.transform);
    hippePoint.transform.localPosition = new Vector3(target.transform.position.x, target.transform.position.y + 15f, target.transform.position.z);
    hippePoint.transform.localScale = new Vector3(45f, 45f, 1f);
    hippePoint.GetComponent<Animator>().SetTrigger("arti");

    Destroy(hippePoint, 2f);
  }
}