using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
  public GameObject SubScreen;
  public GameObject PauseDialog;
  public GameObject WinDialog;
  public GameObject LooseDialog;
  public GameObject Health;
  public GameObject Gold;
  public GameObject Wave;


  private void openControl()
  {
    SubScreen.SetActive(true);
    Health.SetActive(false);
    Gold.SetActive(false);
    Wave.SetActive(false);
  }

  private void closeControl()
  {
    SubScreen.SetActive(false);
    Health.SetActive(true);
    Gold.SetActive(true);
    Wave.SetActive(true);
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
