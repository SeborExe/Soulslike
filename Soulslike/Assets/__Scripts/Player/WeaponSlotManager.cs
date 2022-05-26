using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    WeaponHolderSlot leftHandSlot;
    WeaponHolderSlot rightHandSlot;
    PlayerManager playerManager;
    PlayerStats playerStats;
    InputHandler inputHandler;
    PlayerInventory playerInventory;
    Animator animator;
    QuickSlotUI quickSlotsUI;

    public DamageCollider leftHandDamageCollider;
    public DamageCollider rightHandDamageCollider;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        animator = GetComponent<Animator>();
        quickSlotsUI = FindObjectOfType<QuickSlotUI>();
        playerStats = GetComponentInParent<PlayerStats>();
        inputHandler = GetComponentInParent<InputHandler>();

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
            //else if (weaponSlot.isBackSlot)
            //{
            //    backSlot = weaponSlot;
            //}
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponCollider();
            quickSlotsUI.UpdateWeaponSlotsUI(true, weaponItem);

            #region Handle weapon idle animations
            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.Left_Hand_Idle, 0.2f);
            }
            else
            {
                animator.CrossFade("Left Arm Empty", 0.2f);
            }
            #endregion
        }
        else
        {
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponCollider();
            quickSlotsUI.UpdateWeaponSlotsUI(false, weaponItem);

            #region Handle weapon idle animations

            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.Right_Hand_Idle, 0.2f);
            }
            else
            {
                animator.CrossFade("Right Arm Empty", 0.2f);
            }
            #endregion
        }

    }

    #region Open and Close weapon collider

    private void LoadLeftWeaponCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        if (leftHandDamageCollider != null)
            leftHandDamageCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
    }

    private void LoadRightWeaponCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        if (rightHandDamageCollider != null)
            rightHandDamageCollider.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;
    }

    public void OpenDamageCollider()
    {
        //if (playerManager.isUsingRightHand && rightHandDamageCollider != null)
        if (rightHandDamageCollider != null)
        {
            rightHandDamageCollider.EnabelDamageCollider();
        }
        //else if (playerManager.isUsingLeftHand && leftHandDamageCollider != null)
        else if (leftHandDamageCollider != null)
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
}
