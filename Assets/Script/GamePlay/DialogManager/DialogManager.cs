using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
  public static bool isGamePaused = false;
  public GameObject SubScreen;
  public GameObject PauseDialog;
  public GameObject WinDialog;
  public GameObject LooseDialog;
  public GameObject FadeBackground;

  private void openControl()
  {
    SubScreen.SetActive(true);
    FadeBackground.SetActive(true);
    isGamePaused = true;
  }

  private void closeControl()
  {
    SubScreen.SetActive(false);
    FadeBackground.SetActive(false);
    Time.timeScale = 1f;
    isGamePaused = false;
  }

  public void handlOpenPauseDialog()
  {
    PauseDialog.SetActive(true);
    openControl();
  }

  public void handleClosePauseDialog()
  {
    PauseDialog.SetActive(false);
    closeControl();
  }

  public void handlOpenWinDialog()
  {
    WinDialog.SetActive(true);
    openControl();
  }

  public void handleCloseWinDialog()
  {
    WinDialog.SetActive(false);
    closeControl();
  }

  public void handlOpenLooseDialog()
  {
    LooseDialog.SetActive(true);
    openControl();
  }

  public void handleCloseLooseDialog()
  {
    LooseDialog.SetActive(false);
    closeControl();
  }
}
