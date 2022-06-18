using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    [Header("Inputs")]
    public bool b_Input;
    public bool rb_Input;
    public bool rt_Input;
    public bool lt_Input;
    public bool lb_Input;
    public bool d_pad_up;
    public bool d_pad_down;
    public bool d_pad_left;
    public bool d_pad_right;
    public bool a_Input;
    public bool jump_Input;
    public bool inventory_Input;
    public bool lockOn_Input;
    public bool right_Stick_Right_Input;
    public bool right_Stick_Left_Input;
    public bool y_Input;
    public bool critical_attack_Input;
    public bool x_Input;

    [Header("Flags")]
    public float rollInputTimer;
    public bool rollFlag;
    public bool sprintFlag;
    public bool lockOnFlag;
    public bool comboFlag;
    public bool inventoryFlag;
    public bool twoHandFlag;

    PlayerControls inputActions;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerManager playerManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerCombatManager playerCombatManager;
    UIManager uIManager;
    CameraHandler cameraHandler;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    PlayerStatsManager playerStatsManager;
    BlockingCollider blockingCollider;
    PlayerEffectsManager playerEffectsManager;

    Vector2 movementInput;
    Vector2 cameraInput;

    public Transform criticalAttackRaycastStartPoint;

    private void Awake()
    {
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerManager = GetComponent<PlayerManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        blockingCollider = GetComponentInChildren<BlockingCollider>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        cameraHandler = FindObjectOfType<CameraHandler>();
        uIManager = FindObjectOfType<UIManager>();
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();

            //Movement
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
            inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;

            //Actions
            inputActions.PlayerActions.Roll.performed += i => b_Input = true;
            inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
            inputActions.PlayerQuickSlot.DPadRight.performed += i => d_pad_right = true;
            inputActions.PlayerQuickSlot.DPadLeft.performed += i => d_pad_left = true;
            inputActions.PlayerActions.A.performed += i => a_Input = true;
            inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
            inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
            inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
            inputActions.PlayerActions.Y.performed += i => y_Input = true;
            inputActions.PlayerActions.RB.performed += i => rb_Input = true;
            inputActions.PlayerActions.RT.performed += i => rt_Input = true;
            inputActions.PlayerActions.LT.performed += i => lt_Input = true;
            inputActions.PlayerActions.LB.performed += i => lb_Input = true;
            inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
            inputActions.PlayerActions.CriticalAttack.performed += i => critical_attack_Input = true;
            inputActions.PlayerActions.X.performed += i => x_Input = true;
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        HandleMoveInput(delta);
        HandleRollInput(delta);
        HandleCombatInput(delta);
        HandleQuickSlotInput();
        HandleInventoryWindow();
        HandleLockOnInput();
        HandleTwoHandInput();
        HandleCriticalAttackInput();
        HandleUseConsumableInput();
    }

    public void HandleMoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRollInput(float delta)
    {
        if (b_Input)
        {
            rollInputTimer += delta;

            if (playerStatsManager.currentStamina <= 0)
            {
                b_Input = false;
                sprintFlag = false;
            }

            if (moveAmount > 0.5f && playerStatsManager.currentStamina > 0)
            {
                sprintFlag = true;
            }
        }
        else
        {
            sprintFlag = false;

            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    private void HandleCombatInput(float delta)
    {
        if (rb_Input)
        {
            playerCombatManager.HandleRBAction();
        }

        if (rt_Input)
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.canDoCombo)
                return;

            playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
            playerCombatManager.HandleHeavyAttack(playerInventoryManager.rightWeapon);
        }

        if (lb_Input)
        {
            playerCombatManager.HandleLBAction();
        }
        else
        {
            playerManager.isBlocking = false;

            if (blockingCollider.blockingCollider.enabled)
            {
                blockingCollider.DisableBlockingCollider();
            }
        }

        if (lt_Input)
        {
            if (twoHandFlag)
            {

            }
            else
            {
                playerCombatManager.HandleLTAction();
            }
        }
    }

    private void HandleQuickSlotInput()
    {
        if (d_pad_right)
        {
            playerInventoryManager.ChangeRightWeapon();
        }

        else if (d_pad_left)
        {
            playerInventoryManager.ChangeLeftWeapon();
        }
    }

    private void HandleInventoryWindow()
    {
        if (inventory_Input)
        {
            inventoryFlag = !inventoryFlag;

            if (inventoryFlag)
            {
                uIManager.OpenSelectWindow();
                uIManager.UpdateUI();
                uIManager.hudWindow.SetActive(false);
            }
            else
            {
                uIManager.CloseSelectWindow();
                uIManager.CloseAllInventoryWindows();
                uIManager.hudWindow.SetActive(true);
            }
        }
    }

    private void HandleLockOnInput()
    {
        if (lockOn_Input && !lockOnFlag)
        {
            lockOn_Input = false;
            cameraHandler.HandleLockOn();

            if (cameraHandler.nearestLockOnTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                lockOnFlag = true;
            }
        }

        else if (lockOn_Input && lockOnFlag)
        {
            lockOn_Input = false;
            lockOnFlag = false;
            cameraHandler.ClearLockOnTargets();
        }

        
        if (lockOnFlag && right_Stick_Left_Input)
        {
            right_Stick_Left_Input = false;
            cameraHandler.HandleLockOn();
            if (cameraHandler.leftLockTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
            }
        }

        if (lockOnFlag && right_Stick_Right_Input)
        {
            right_Stick_Right_Input = false;
            cameraHandler.HandleLockOn();
            if (cameraHandler.rightLockTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
            }
        }
        

        cameraHandler.SetCameraHeight();
    }

    private void HandleTwoHandInput()
    {
        if (y_Input)
        {
            y_Input = false;

            twoHandFlag = !twoHandFlag;

            if (twoHandFlag)
            {
                playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                playerManager.isTwoHandWeapon = true;
            }
            else
            {
                playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
                playerManager.isTwoHandWeapon = false;
            }
        }
    }

    private void HandleCriticalAttackInput()
    {
        if (critical_attack_Input)
        {
            critical_attack_Input = false;
            playerCombatManager.AttemptBackStabOrRipost();
        }
    }

    private void HandleUseConsumableInput()
    {
        if (playerStatsManager.isDead) return;

        if (x_Input)
        {
            x_Input = false;
            playerInventoryManager.currentConsumableItem.AttemptToConsumableItem(playerAnimatorManager, playerWeaponSlotManager, playerEffectsManager);
        }
    }
}
