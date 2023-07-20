using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
  public static SpawnEnemies instance;
  private const float waitingTime = 2f;
  public GameObject spawnPointUp;
  public GameObject spawnPointDown;
  public GameObject[] enemiesUpWave;
  public int numberOfEnemiesUpWave;
  public GameObject[] enemiesDownWave;
  public int numberOfEnemiesDownWave;
  public int maxEnemiesOnWave;
  public int enemiesOnScreen;
  public int totalEnemies;
  public int enemiesPerSpawn;

  public GameObject HPBarragePrefab;
  public GameObject HPBarragePrefabLeft;
  public GameObject BarragePrefab;
  public float hpBarUpperWaveVerticalOffset = 1.3f;
  public float hpBarDownWaveVerticalOffset = 1.22f;
  public GameObject Wave;
  public GameSystem gameSystem;
  private void Awake()
  {
    instance = this;
  }

  void Start()
  {
    StartCoroutine(SpawnWave(enemiesUpWave, numberOfEnemiesUpWave, spawnPointUp, true));
    StartCoroutine(SpawnWave(enemiesDownWave, numberOfEnemiesDownWave, spawnPointDown, false));
    gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
  }

  IEnumerator SpawnWave(GameObject[] enemies, int numberOfEnemies, GameObject spawnPoint, bool isUpperWave)
  {
    int spawnedEnemies = 0;
    GameObject enemiesParent = GameObject.Find("Canvas/Manager/Enemies");
    float waveWaitingTime = isUpperWave ? waitingTime * 1.25f : waitingTime * 1.5f;

    while (spawnedEnemies < numberOfEnemies)
    {
      yield return new WaitForSeconds(waveWaitingTime);

      for (int i = 0; i < enemiesPerSpawn; i++)
      {
        if (spawnedEnemies < numberOfEnemies && enemiesOnScreen < maxEnemiesOnWave)
        {
          GameObject enemyPrefab = enemies[Random.Range(0, enemies.Length)] as GameObject;
          GameObject newEnemy = PrefabUtility.InstantiatePrefab(enemyPrefab) as GameObject;
          Vector3 newEnemyPos = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, 0f);
          newEnemy.transform.SetParent(enemiesParent.transform);

          GameObject newHPBarrage = Instantiate(HPBarragePrefab);
          GameObject newHPBarrageL = Instantiate(HPBarragePrefabLeft);
          GameObject newBarrage = Instantiate(BarragePrefab);
          newHPBarrageL.transform.SetParent(newEnemy.transform);
          newHPBarrage.transform.SetParent(newEnemy.transform);
          newBarrage.transform.SetParent(newEnemy.transform);

          float verticalOffset = isUpperWave ? hpBarUpperWaveVerticalOffset : hpBarDownWaveVerticalOffset;
          newHPBarrageL.transform.localPosition = new Vector3(0, verticalOffset, 0);
          newHPBarrage.transform.localPosition = new Vector3(0, verticalOffset, 0.1f);
          newBarrage.transform.localPosition = new Vector3(0, 0, 0f);
          newBarrage.transform.localScale = new Vector3(1f, 1f, 1f);

          newEnemy.GetComponent<EnemyStatus>().bloodEffect = newBarrage.gameObject;
          newEnemy.GetComponent<EnemyStatus>().HPBarrageLeft = newHPBarrageL;

          EnemyStatus enemyStatus = newEnemy.GetComponent<EnemyStatus>();
          if (enemyStatus != null)
          {
            enemyStatus.HPBarrage = newHPBarrage;
          }
          enemiesOnScreen++;
          spawnedEnemies++;
          gameSystem.EnemiesIncrease();
          newEnemy.transform.position = newEnemyPos;
          newEnemy.transform.localPosition = newEnemyPos;
        }
      }

      if (spawnedEnemies >= numberOfEnemies || enemiesOnScreen >= totalEnemies)
      {
        if (enemiesOnScreen == totalEnemies)
        {
          Wave.SetActive(false);
        }
        break;
      }
    }
  }
}