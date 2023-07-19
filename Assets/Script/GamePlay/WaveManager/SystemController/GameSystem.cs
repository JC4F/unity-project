using UnityEngine;
using TMPro;

public class GameSystem : MonoBehaviour
{
  public TextMeshProUGUI lives;
  public TextMeshProUGUI gold;
  public TextMeshProUGUI wavesText;

  public int goldValue;
  public int liveValue;
  public int waveCurrent;
  public int totalWaves;
  float startTime;
  public int timerDuration;
  public float[] waveDelays;
  public GameObject[] waves;
  private GameObject timeSkipWave;
  private Animator skipCloseLeft;
  private Animator skipCloseRight;
  private bool canSkipNow = false;
  private DialogManager dialogManager;

  void Start()
  {
    //Variable
    skipCloseLeft = GameObject.FindGameObjectWithTag("SkipCloseLeft").GetComponent<Animator>();
    skipCloseRight = GameObject.FindGameObjectWithTag("SkipCloseRight").GetComponent<Animator>();
    timeSkipWave = GameObject.Find("TimeSkip");
    dialogManager = GameObject.FindGameObjectWithTag("DialogManager").GetComponent<DialogManager>();
    //Method
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
    else
    {
      CheckCurrentWave();
    }
  }

  public void StaringSkipWave()
  {
    skipCloseLeft.SetTrigger("openLeft");
    skipCloseRight.SetTrigger("openRight");
  }

  public void CloseSkipWave()
  {
    canSkipNow = true;
    skipCloseLeft.SetTrigger("closeLeft");
    skipCloseRight.SetTrigger("closeRight");
  }

  public void CheckCurrentWave()
  {
    if (waveCurrent != 0 && waveCurrent < totalWaves && canSkipNow)
    {
      float elapsedTime = Time.time - startTime;

      if (elapsedTime >= timerDuration)
      {
        if (waves[waveCurrent] != null)
        {
          GameObject nextWaveObject = waves[waveCurrent];
          nextWaveObject.SetActive(true);
          Transform nextWaveObjectChild = nextWaveObject.transform.Find("WavePoint01/Background01");
          timeSkipWave.GetComponent<StaringWave>().descriptionPanel = nextWaveObjectChild.GetComponent<StaringWave>().descriptionPanel;
          timeSkipWave.GetComponent<StaringWave>().spawnEnemies = nextWaveObjectChild.GetComponent<StaringWave>().spawnEnemies;
          timeSkipWave.GetComponent<StaringWave>().waveFade = nextWaveObjectChild.GetComponent<StaringWave>().waveFade;
          StaringSkipWave();
          canSkipNow = false;
          startTime = Time.time;
        }
      }
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
    wavesText.text = "WAVE " + waveCurrent.ToString() + "/" + totalWaves.ToString();
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
    dialogManager.handlOpenLooseDialog();
  }

  void AllWavesCompleted()
  {
    dialogManager.handlOpenWinDialog();
  }
}