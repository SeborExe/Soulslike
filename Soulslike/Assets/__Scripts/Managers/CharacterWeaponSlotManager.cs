using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponSlotManager : MonoBehaviour
{
    CharacterManager characterManager;
    CharacterAnimatorManager characterAnimatorManager;
    CharacterInventoryManager characterInventoryManager;
    CharacterStatsManager characterStatsManager;
    CharacterEffectsManager characterEffectsManager;

    [Header("Unarmed weapon")]
    public WeaponItem unarmedWeapon;

    [Header("Weapon slots")]
    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;
    public WeaponHolderSlot backSlot;

    [Header("Damage colliders")]
    public DamageCollider leftHandDamageCollider;
    public DamageCollider rightHandDamageCollider;

    private void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterInventoryManager = GetComponent<CharacterInventoryManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
    }

    protected virtual void LoadWeaponHolderSlots()
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

    public virtual void LoadBothWeaponsOnSlot()
    {
        LoadWeaponOnSlot(characterInventoryManager.rightWeapon, false);
        LoadWeaponOnSlot(characterInventoryManager.leftWeapon, true);
    }

    public virtual void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (weaponItem != null)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponCollider();
                characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                if (characterManager.isTwoHandWeapon)
                {
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    characterAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                }
                else
                {
                    backSlot.UnloadWeaponAndDestroy();
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponCollider();
                characterAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
            }
        }
        else
        {
            weaponItem = unarmedWeapon;

            if (isLeft)
            {
                characterInventoryManager.leftWeapon = unarmedWeapon;
                leftHandSlot.currentWeapon = unarmedWeapon;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponCollider();
                characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                characterInventoryManager.rightWeapon = unarmedWeapon;
                rightHandSlot.currentWeapon = unarmedWeapon;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponCollider();
                characterAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
            }
        }
    }

    protected virtual void LoadLeftWeaponCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        if (leftHandDamageCollider != null)
        {
            leftHandDamageCollider.physicalDamage = characterInventoryManager.leftWeapon.physicalDamage;
            leftHandDamageCollider.fireDamage = characterInventoryManager.leftWeapon.fireDamage;

            leftHandDamageCollider.teamIDNumber = characterStatsManager.teamIDNumber;

            leftHandDamageCollider.poiseBreak = characterInventoryManager.leftWeapon.poiseBreak;
            characterEffectsManager.leftWeaponWF = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
    }

    protected virtual void LoadRightWeaponCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        if (rightHandDamageCollider != null)
        {
            rightHandDamageCollider.physicalDamage = characterInventoryManager.rightWeapon.physicalDamage;
            rightHandDamageCollider.fireDamage = characterInventoryManager.rightWeapon.fireDamage;

            rightHandDamageCollider.teamIDNumber = characterStatsManager.teamIDNumber;

            rightHandDamageCollider.poiseBreak = characterInventoryManager.rightWeapon.poiseBreak;
            characterEffectsManager.rightWeaponWF = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
    }

    public virtual void OpenDamageCollider()
    {
        if (characterManager.isUsingRightHand && rightHandDamageCollider != null)
        {
            rightHandDamageCollider.EnabelDamageCollider();
        }
        else if (characterManager.isUsingLeftHand && leftHandDamageCollider != null)
        {
            leftHandDamageCollider.EnabelDamageCollider();
        }
    }

    public virtual void CloseDamageCollider()
    {
        if (rightHandDamageCollider != null)
            rightHandDamageCollider.DisableDamageCollider();

        if (leftHandDamageCollider != null)
            leftHandDamageCollider.DisableDamageCollider();
    }
}
