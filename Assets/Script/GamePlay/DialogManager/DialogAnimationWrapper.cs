using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogAnimationWrapper : MonoBehaviour
{
  private Animator animator;

  private void Start()
  {
    animator = GetComponent<Animator>();
  }

  public void OnAnimationComplete()
  {
    Debug.Log("hihi");
    if (DialogManager.isGamePaused)
    {
      Time.timeScale = 0f;
    }
  }
}
