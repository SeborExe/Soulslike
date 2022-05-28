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
    public bool d_pad_up;
    public bool d_pad_down;
    public bool d_pad_left;
    public bool d_pad_right;
    public bool a_Input;
    public bool jump_Input;
    public bool inventory_Input;
    public bool lockOn_Input;

    [Header("Flags")]
    public float rollInputTimer;
    public bool rollFlag;
    public bool sprintFlag;
    public bool lockOnFlag;
    public bool comboFlag;
    public bool inventoryFlag;

    PlayerControls inputActions;
    PlayerAnimatorManager animationHandler;
    PlayerManager playerManager;
    PlayerInventory playerInventory;
    PlayerAttacker playerAttacker;
    UIManager uIManager;
    CameraHandler cameraHandler;
    WeaponSlotManager weaponSlotManager;
    PlayerStats playerStats;

    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        playerAttacker = GetComponentInChildren<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        uIManager = FindObjectOfType<UIManager>();
        animationHandler = GetComponentInChildren<PlayerAnimatorManager>();
        cameraHandler = FindObjectOfType<CameraHandler>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();

            //Movement
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            //Actions
            inputActions.PlayerActions.Roll.performed += i => b_Input = true;
            inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
            inputActions.PlayerQuickSlot.DPadRight.performed += i => d_pad_right = true;
            inputActions.PlayerQuickSlot.DPadLeft.performed += i => d_pad_left = true;
            inputActions.PlayerActions.A.performed += i => a_Input = true;
            inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
            inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
            inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
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
        HandleAttackInput();
        HandleQuickSlotInput();
        HandleInventoryWindow();
        HandleLockOnInput();
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
            sprintFlag = true;
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    private void HandleAttackInput()
    {
        inputActions.PlayerActions.RB.performed += i => rb_Input = true;
        inputActions.PlayerActions.RT.performed += i => rt_Input = true;
        
        if (rb_Input)
        {
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting) return;

                if (playerManager.canDoCombo) return;

                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        if (rt_Input)
        {
            if (playerManager.isInteracting) return;

            if (playerManager.canDoCombo) return;

            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
        }
    }

    private void HandleQuickSlotInput()
    {
        if (d_pad_right)
        {
            playerInventory.ChangeRightWeapon();
        }

        else if (d_pad_left)
        {
            playerInventory.ChangeLeftWeapon();
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

        /*
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
        */

        //cameraHandler.SetCameraHeight();
    }
    
}
