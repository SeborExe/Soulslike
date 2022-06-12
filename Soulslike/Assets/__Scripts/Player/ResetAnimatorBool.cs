using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string isInteractingBool = "isInteracting";
    public bool isInteractingStatus = false;

    public string isFiringSpellBool = "isFiringSpell";
    public bool isFiringSpellStatus = false;

    public string canRotateBool = "canRotate";
    public bool canRotateStatus = true;

    public string isRotatingWithRootMotionBool = "isRotatingWithRootMotion";
    public bool isRotatingWithRootMotionStatus = false;

    public string isInvulnerableBool = "isInvulnerable";
    public bool isInvulnerableStatus = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBool, isInteractingStatus);
        animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
        animator.SetBool(isRotatingWithRootMotionBool, isRotatingWithRootMotionStatus);
        animator.SetBool(canRotateBool, canRotateStatus);
        animator.SetBool(isInvulnerableBool, isInvulnerableStatus);
    }
}
