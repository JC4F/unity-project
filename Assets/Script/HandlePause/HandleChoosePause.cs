using UnityEngine.EventSystems;
using UnityEngine;

public class HandleChoosePause : MonoBehaviour
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
