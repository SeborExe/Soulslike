using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;

    EnemyStatsManager enemyStatsManager;
    EnemyEffectManager enemyEffectManager;

    private void Awake()
    {
        enemyStatsManager = GetComponent<EnemyStatsManager>();
        enemyEffectManager = GetComponent<EnemyEffectManager>();
        LoadWeaponHolderSlots();
    }

    private void Start()
    {
        LoadWeaponsOnBothHands();
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
        }
    }

    private void LoadWeaponsOnBothHands()
    {
        if (rightHandWeapon != null)
        {
            LoadWeaponOnSlot(rightHandWeapon, false);
        }

        if (leftHandWeapon != null)
        {
            LoadWeaponOnSlot(leftHandWeapon, true);
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.currentWeapon = weapon;
            leftHandSlot.LoadWeaponModel(weapon);
            LoadWeaponDamageCollider(true);
        }
        else
        {
            rightHandSlot.currentWeapon = weapon;
            rightHandSlot.LoadWeaponModel(weapon);
            LoadWeaponDamageCollider(false);
        }
    }

    public void LoadWeaponDamageCollider(bool isLeft)
    {
        if (isLeft)
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();

            leftHandDamageCollider.physicalDamage = leftHandWeapon.physicalDamage;
            leftHandDamageCollider.fireDamage = leftHandWeapon.fireDamage;

            enemyEffectManager.leftWeaponWF = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        else
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();

            leftHandDamageCollider.physicalDamage = rightHandWeapon.physicalDamage;
            leftHandDamageCollider.fireDamage = rightHandWeapon.fireDamage;

            enemyEffectManager.rightWeaponWF = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
    }

    public void OpenDamageCollider()
    {
        rightHandDamageCollider.EnabelDamageCollider();
    }

    public void CloseDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    #region DrainStaminaAndCombo
    
    public void DrainStaminaLightAttack()
    {

    }

    public void DrainStaminaHeavyAttack()
    {

    }
    
    public void EnableCombo()
    {
        //anim.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        //anim.SetBool("canDoCombo", false);
    }

    #endregion

    #region Handle Weapons Poise Bonus

    public void GrantWeaponAttackingPoiseBonus()
    {
        enemyStatsManager.totalPoiseDefense += enemyStatsManager.offensivePoiseBonus;
    }

    public void ResetWeaponAttackingPoiseBonus()
    {
        enemyStatsManager.totalPoiseDefense = enemyStatsManager.armorPoiseBonus;
    }

    #endregion
}
