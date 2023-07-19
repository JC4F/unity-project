using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationFadeIn : MonoBehaviour
{
    //Serialize = public
    public RawImage rawImage;
    public bool verification;
    private void Start()
    {
        if (PlayerPrefs.GetInt("shouldApplyFade", 1) == 0 && verification)
        {
            rawImage.gameObject.SetActive(false);
        }
    }
    
    public void OnFadeIn()
    {
        rawImage.gameObject.SetActive(false);
    }
}
