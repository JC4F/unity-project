using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBehavior : StateMachineBehaviour
{
    bool isAnimationStarted;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (animator.GetBool("StartAnimation"))
        {
            if (!isAnimationStarted)
            {
                animator.enabled = false;
                isAnimationStarted = true;
            }
        }
    }
}
