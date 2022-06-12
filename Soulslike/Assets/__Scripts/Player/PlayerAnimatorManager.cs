using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : AnimatorManager
{
    InputHandler inputHandler;
    PlayerLocomotionManager playerLocomotionManager;
    PlayerManager playerManager;

    int vertical;
    int horizontal;

    public void Initialize()
    {
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        inputHandler = GetComponent<InputHandler>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical

        float v = 0;

        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }

        else if (verticalMovement > 0.55f)
        {
            v = 1f;
        }

        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }

        else if (verticalMovement < -0.55f)
        {
            v = -1f;
        }
        else
        {
            v = 0;
        }

        #endregion

        #region Horizontal

        float h = 0;

        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }

        else if (horizontalMovement > 0.55f)
        {
            h = 1f;
        }

        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }

        else if (horizontalMovement < -0.55f)
        {
            h = -1f;
        }
        else
        {
            h = 0;
        }

        #endregion

        if (isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }

        animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void CanRotate()
    {
        animator.SetBool("canRotate", true);
    }

    public void StopRotation()
    {
        animator.SetBool("canRotate", false);
    }

    public void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        animator.SetBool("canDoCombo", false);
    }

    public void EnableIsInvulnerable()
    {
        animator.SetBool("isInvulnerable", true);
    }

    public void DisableIsInbulnerable()
    {
        animator.SetBool("isInvulnerable", false);
    }

    public void EnableIsParrying()
    {
        playerManager.isParrying = true;
    }

    public void DisableIsParrying()
    {
        playerManager.isParrying = false;
    }

    public void EnableCanBeReposted()
    {
        playerManager.canBeReposted = true;
    }

    public void DisableCanBeReposed()
    {
        playerManager.canBeReposted = false;
    }

    public void EnableRepost()
    {
        playerManager.canBeReposted = true;
    }

    public void DesableRepost()
    {
        playerManager.canBeReposted = false;
    }

    public void DisableCollision()
    {
        playerLocomotionManager.characterCollider.enabled = false;
        playerLocomotionManager.characterCollisionBlocker.enabled = false;
    }

    public void EnableCollision()
    {
        playerLocomotionManager.characterCollider.enabled = true;
        playerLocomotionManager.characterCollisionBlocker.enabled = true;
    }

    private void OnAnimatorMove()
    {
        if (playerManager.isInteracting == false) return;

        float delta = Time.deltaTime;
        playerLocomotionManager.rigidbody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerLocomotionManager.rigidbody.velocity = velocity;
    }
}
