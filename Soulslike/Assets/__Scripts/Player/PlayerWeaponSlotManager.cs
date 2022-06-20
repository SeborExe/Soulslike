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
    CameraHandler cameraHandler;
    PlayerAnimatorManager playerAnimatorManager;

    [Header("Attacking weapon")]
    public WeaponItem attackingWeapon;

    private void Awake()
    {
        cameraHandler = FindObjectOfType<CameraHandler>();

        inputHandler = GetComponent<InputHandler>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        animator = GetComponent<Animator>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        quickSlotsUI = FindObjectOfType<QuickSlotUI>();

        LoadWeaponHolderSlots();
        LoadBothWeaponsOnSlot();
    }

    public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (weaponItem != null)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponCollider();
                quickSlotsUI.UpdateWeaponSlotsUI(true, weaponItem);
                playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    leftHandSlot.UnloadWeaponAndDestroy();
                    playerAnimatorManager.PlayTargetAnimation("Left Arm Empty", false, true);
                }
                else
                {
                    backSlot.UnloadWeaponAndDestroy();
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponCollider();
                quickSlotsUI.UpdateWeaponSlotsUI(false, weaponItem);
                playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController; 
            }
        }
        else
        {
            weaponItem = unarmedWeapon;

            if (isLeft)
            {
                playerInventoryManager.leftWeapon = unarmedWeapon;
                leftHandSlot.currentWeapon = unarmedWeapon;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponCollider();
                quickSlotsUI.UpdateWeaponSlotsUI(true, weaponItem);
                playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
            }
            else
            {
                playerInventoryManager.rightWeapon = unarmedWeapon;
                rightHandSlot.currentWeapon = unarmedWeapon;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponCollider();
                quickSlotsUI.UpdateWeaponSlotsUI(false, weaponItem);
                playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
            }
        }
    }

    public void SuccessfulyThrowFireBomb()
    {
        Destroy(playerEffectsManager.instantiateFXModel);
        BombConsumableItem fireBombItem = playerInventoryManager.currentConsumableItem as BombConsumableItem;

        Vector3 startPos = playerStatsManager.GetComponentInChildren<ProjectileStartingPos>().transform.position;

        GameObject activeModelBomb = Instantiate(fireBombItem.liveBombModel, startPos,
            cameraHandler.cameraPivotTransform.rotation);
        activeModelBomb.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x,
            playerManager.lockOnTransform.eulerAngles.y, 0);
        BombDamageCollider damageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();

        damageCollider.explosionDamage = fireBombItem.baseDamage;
        damageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;
        damageCollider.bombRigidbody.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
        damageCollider.bombRigidbody.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelicoty);
        damageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
        LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
        //create explosion and deal damage
    }

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
