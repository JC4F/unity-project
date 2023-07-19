using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBBG : StateMachineBehaviour
{
    bool isAnimationStarted;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (animator.GetBool("BBGAnimation"))
        {
            if (!isAnimationStarted)
            {
                animator.enabled = false;
                isAnimationStarted = true;
            }
        }
    }
}
