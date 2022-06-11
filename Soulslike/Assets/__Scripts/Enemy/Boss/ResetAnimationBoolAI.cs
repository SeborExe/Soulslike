using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimationBoolAI : ResetAnimatorBool
{

    public string isPhaseShiftingBool = "isPhaseShifting";
    public bool isPhaseShiftingStatus = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetBool(isPhaseShiftingBool, isPhaseShiftingStatus);
    }
}
