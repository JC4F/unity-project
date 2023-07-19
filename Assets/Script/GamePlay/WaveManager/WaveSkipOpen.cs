using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSkipOpen : MonoBehaviour
{
    public void openSkipWave()
    {
        GameSystem gameSystem = GameObject.FindGameObjectWithTag("GameSystem").GetComponent<GameSystem>();
        gameSystem.StaringSkipWave();
    }
}
