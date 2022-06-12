using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
{
    InputHandler inputHandler;
    PlayerManager playerManager;
    PlayerStatsManager playerStatsManager;
    PlayerInventoryManager playerInventoryManager;
    Animator animator;
    QuickSlotUI quickSlotsUI;
    PlayerEffectsManager playerEffectsManager;

    [Header("Attacking weapon")]
    public WeaponItem attackingWeapon;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        animator = GetComponent<Animator>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        quickSlotsUI = FindObjectOfType<QuickSlotUI>();

        LoadWeaponHolderSlots();
        LoadBothWeaponsOnSlot();
    }

    private void LoadWeaponHolderSlots()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.isBackSlot)
            {
                backSlot = weaponSlot;
            }
        }
    }

    public void LoadBothWeaponsOnSlot()
    {
        LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
        LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (weaponItem != null)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponCollider();
                quickSlotsUI.UpdateWeaponSlotsUI(true, weaponItem);
                animator.CrossFade(weaponItem.Left_Hand_Idle, 0.2f);
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.Two_Hand_Idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Both Arms Empty", 0.2f);
                    backSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.Right_Hand_Idle, 0.2f);
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponCollider();
                quickSlotsUI.UpdateWeaponSlotsUI(false, weaponItem);
            }
        }
        else
        {
            weaponItem = unarmedWeapon;

            if (isLeft)
            {
                animator.CrossFade("Left Arm Empty", 0.2f);
                playerInventoryManager.leftWeapon = unarmedWeapon;
                leftHandSlot.currentWeapon = unarmedWeapon;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponCollider();
                quickSlotsUI.UpdateWeaponSlotsUI(true, weaponItem);
            }
            else
            {
                animator.CrossFade("Right Arm Empty", 0.2f);
                playerInventoryManager.rightWeapon = unarmedWeapon;
                rightHandSlot.currentWeapon = unarmedWeapon;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponCollider();
                quickSlotsUI.UpdateWeaponSlotsUI(false, weaponItem);
            }
        }
    }

    #region Open and Close weapon collider

    private void LoadLeftWeaponCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        if (leftHandDamageCollider != null)
        {
            leftHandDamageCollider.currentWeaponDamage = playerInventoryManager.leftWeapon.baseDamage;
            leftHandDamageCollider.poiseBreak = playerInventoryManager.leftWeapon.poiseBreak;
            playerEffectsManager.leftWeaponWF = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
    }

    private void LoadRightWeaponCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        if (rightHandDamageCollider != null)
        {
            rightHandDamageCollider.currentWeaponDamage = playerInventoryManager.rightWeapon.baseDamage;
            rightHandDamageCollider.poiseBreak = playerInventoryManager.rightWeapon.poiseBreak;
            playerEffectsManager.rightWeaponWF = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
    }

    public void OpenDamageCollider()
    {
        if (playerManager.isUsingRightHand && rightHandDamageCollider != null)
        {
            rightHandDamageCollider.EnabelDamageCollider();
        }
        else if (playerManager.isUsingLeftHand && leftHandDamageCollider != null)
        {
            leftHandDamageCollider.EnabelDamageCollider();
        }
    }

    public void CloseDamageCollider()
    {
        if (rightHandDamageCollider != null)
            rightHandDamageCollider.DisableDamageCollider();

        if (leftHandDamageCollider != null)
            leftHandDamageCollider.DisableDamageCollider();
    }

    #endregion

    #region DrainStamina
    public void DrainStaminaLightAttack()
    {
        playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
    #endregion

    #region Handle Weapons Poise Bonus

    public void GrantWeaponAttackingPoiseBonus()
    {
        playerStatsManager.totalPoiseDefense += attackingWeapon.offensivePoiseBonus;
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
        playerStatsManager.totalPoiseDefense = playerStatsManager.armorPoiseBonus;
    }

    #endregion
}
