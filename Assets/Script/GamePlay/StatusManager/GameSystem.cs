using UnityEngine;
using TMPro;

public class GameSystem : MonoBehaviour
{
  public TextMeshProUGUI lives;
  public TextMeshProUGUI gold;
  public TextMeshProUGUI waves;

  public int goldValue;
  public int liveValue;
  public int waveCurrent;
  public int totalWaves;

  void Start()
  {
    UpdateGoldText();
    UpdateLivesText();
    UpdateWavesText();
  }

  void Update()
  {
    if (liveValue <= 0)
    {
      GameOver();
    }

    if (waveCurrent >= totalWaves)
    {
      AllWavesCompleted();
    }
  }

  public void SpendGold(int amount)
  {
    goldValue -= amount;
    UpdateGoldText();
  }

  void UpdateGoldText()
  {
    gold.text = goldValue.ToString();
  }

  void UpdateLivesText()
  {
    lives.text = liveValue.ToString();
  }

  void UpdateWavesText()
  {
    waves.text = "WAVE " + waveCurrent.ToString() + "/" + totalWaves.ToString();
  }

  public void EarnGold(int amount)
  {
    goldValue += amount;
    UpdateGoldText();
  }

  public void IncreaseWave()
  {
    waveCurrent++;
    UpdateWavesText();
  }

  public void LoseLife(int amount)
  {
    liveValue -= amount;
    UpdateLivesText();
  }

  void GameOver()
  {
    Debug.Log("Game Over!");
  }

  void AllWavesCompleted()
  {
    Debug.Log("All waves completed!");
  }
}