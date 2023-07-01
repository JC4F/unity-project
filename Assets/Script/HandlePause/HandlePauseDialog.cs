using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePauseDialog : MonoBehaviour
{
  public GameObject SubScreen;
  public GameObject PauseDialog;
  public GameObject Health;
  public GameObject Gold;
  public GameObject Wave;


  public void OnMouseDown()
  {
    Debug.Log("PointerClick");
    PauseDialog.SetActive(true);
    SubScreen.SetActive(true);
    Health.SetActive(false);
    Gold.SetActive(false);
    Wave.SetActive(false);
  }
}
