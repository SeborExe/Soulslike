using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    CameraHandler cameraHandler;
    PlayerLocomotion playerLocomotion;
    Animator anim;
    PlayerAnimatorManager playerAnimatorManager;

    [Header("Player flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;
    public bool isInvulnerable;

    private void Awake()
    {
        cameraHandler = FindObjectOfType<CameraHandler>();
        //backStabCollider = GetComponentInChildren<BackStabCollider>();
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        //playerStats = GetComponent<PlayerStats>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        //interactableUI = FindObjectOfType<InteractableUI>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");
        //isUsingRightHand = anim.GetBool("isUsingRightHand");
        //isUsingLeftHand = anim.GetBool("isUsingLeftHand");
        //isInvulnerable = anim.GetBool("isInvulnerable");
        //anim.SetBool("isInAir", isInAir);
        //anim.SetBool("isDead", playerStats.isDead);
        //playerAnimatorManager.canRotate = anim.GetBool("canRotate");

        inputHandler.TickInput(delta);
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleRollingAndSprinting();
        playerLocomotion.HandleFall(delta, playerLocomotion.moveDirection);
        //playerLocomotion.HandleJumping();

        //playerStats.RegenerateStamina();

        //CheckForInteractable();
    }

    private void LateUpdate()
    {
        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        inputHandler.rb_Input = false;
        inputHandler.rt_Input = false;
        //inputHandler.d_pad_up = false;
        //inputHandler.d_pad_down = false;
        //inputHandler.d_pad_left = false;
        //inputHandler.d_pad_right = false;
        //inputHandler.a_Input = false;
        //inputHandler.jump_Input = false;
        //inputHandler.inventory_Input = false;

        float delta = Time.deltaTime;
        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }

        if (isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }
    }
}
