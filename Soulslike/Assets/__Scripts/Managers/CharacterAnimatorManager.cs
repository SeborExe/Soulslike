using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimatorManager : MonoBehaviour
{
    public Animator animator;
    protected CharacterManager characterManager;
    protected CharacterStatsManager characterStatsManager;
    public bool canRotate;

    [Header("Animation Rigging")]
    protected RigBuilder rigBuilder;
    public TwoBoneIKConstraint leftHandConstraint;
    public TwoBoneIKConstraint rightHandConstraint;

    protected virtual void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        rigBuilder = GetComponent<RigBuilder>();
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("canRotate", canRotate);
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }

    public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("isRotatingWithRootMotion", true);
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }

    public virtual void CanRotate()
    {
        animator.SetBool("canRotate", true);
    }

    public virtual void StopRotation()
    {
        animator.SetBool("canRotate", false);
    }

    public virtual void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public virtual void DisableCombo()
    {
        animator.SetBool("canDoCombo", false);
    }

    public virtual void EnableIsInvulnerable()
    {
        animator.SetBool("isInvulnerable", true);
    }

    public virtual void DisableIsInbulnerable()
    {
        animator.SetBool("isInvulnerable", false);
    }

    public virtual void EnableIsParrying()
    {
        characterManager.isParrying = true;
    }

    public virtual void DisableIsParrying()
    {
        characterManager.isParrying = false;
    }

    public virtual void EnableCanBeReposted()
    {
        characterManager.canBeReposted = true;
    }

    public virtual void DisableCanBeReposed()
    {
        characterManager.canBeReposted = false;
    }

    public virtual void TakeCriticalDamageEvent()
    {
        characterStatsManager.TakeDamageNoAnimation(characterManager.pendingCriticalDamage, 0);
        characterManager.pendingCriticalDamage = 0;
    }

    public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandIKTarget, LeftHandIKTarget leftHandIKTarget, bool isTwoHandingWeapon)
    {
        if (isTwoHandingWeapon)
        {
            rightHandConstraint.data.target = rightHandIKTarget.transform;
            rightHandConstraint.data.targetPositionWeight = 1;
            rightHandConstraint.data.targetRotationWeight = 1;

            leftHandConstraint.data.target = leftHandIKTarget.transform;
            leftHandConstraint.data.targetPositionWeight = 1;
            leftHandConstraint.data.targetRotationWeight = 1;
        }
        else
        {
            rightHandConstraint.data.target = null;
            leftHandConstraint.data.target = null;
        }

        rigBuilder.Build();
    }

    public virtual void EraseHandIKForWeapon()
    {

    }
}
