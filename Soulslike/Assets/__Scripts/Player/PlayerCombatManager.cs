using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerManager playerManager;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerStatsManager playerStatsManager;
    PlayerInventoryManager playerInventoryManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    PlayerEquipmentManager playerEquipmentManager;
    CameraHandler cameraHandler;

    public string lastAttack;

    LayerMask backStabLayer = 1 << 10;
    LayerMask ripostLayer = 1 << 11;

    private void Awake()
    {
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerManager = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        inputHandler = GetComponent<InputHandler>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        cameraHandler = FindObjectOfType<CameraHandler>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        if (inputHandler.comboFlag)
        {
            playerAnimatorManager.animator.SetBool("canDoCombo", false);

            if (lastAttack == weapon.OH_Light_attack_01)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_attack_02, true);
            }
            else if (lastAttack == weapon.TH_Light_Attack_01)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.TH_Light_Attack_02, true);
            }
        }
    }
   
    public void HandleLightAttack(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        playerWeaponSlotManager.attackingWeapon = weapon;

        if (inputHandler.twoHandFlag)
        {
            playerAnimatorManager.PlayTargetAnimation(weapon.TH_Light_Attack_01, true);
            lastAttack = weapon.TH_Light_Attack_01;
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_attack_01, true);
            lastAttack = weapon.OH_Light_attack_01;
        }
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        playerWeaponSlotManager.attackingWeapon = weapon;

        //if (inputHandler.twoHandFlag)
        //{

        //}
        //else
        //{
            playerAnimatorManager.PlayTargetAnimation(weapon.OH_Heavy_attack_01, true);
            lastAttack = weapon.OH_Heavy_attack_01;
        //}
    }

    #region Input Actions
    public void HandleRBAction()
    {
        if (playerInventoryManager.rightWeapon.isMeleeWeapon)
        {
            PerformRBMeleeAction();
        }

        else if (playerInventoryManager.rightWeapon.isSpellCaster ||
            playerInventoryManager.rightWeapon.isFaithCaster ||
            playerInventoryManager.rightWeapon.isPyroCaster)
        {
            PerformRBMagicAction(playerInventoryManager.rightWeapon);
        }
    }

    public void HandleLTAction()
    {
        if (playerInventoryManager.leftWeapon.isShieldWeapon)
        {
            PerformLTWeaponArt(inputHandler.twoHandFlag);
        }
        else if (playerInventoryManager.leftWeapon.isMeleeWeapon)
        {

        }
    }

    public void HandleLBAction()
    {
        PerformLBBlockAction();
    }

    #endregion

    #region Attack Actions
    private void PerformRBMeleeAction()
    {
        if (playerManager.canDoCombo)
        {
            inputHandler.comboFlag = true;
            HandleWeaponCombo(playerInventoryManager.rightWeapon);
            inputHandler.comboFlag = false;
        }
        else
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.canDoCombo)
                return;

            playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
            HandleLightAttack(playerInventoryManager.rightWeapon);
        }
    }

    private void PerformRBMagicAction(WeaponItem weapon)
    {
        if (playerManager.isInteracting) return;

        if (weapon.isFaithCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
            {
                if (playerStatsManager.currentMana >= playerInventoryManager.currentSpell.manaCost)
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);

                else
                {
                    playerAnimatorManager.PlayTargetAnimation("Shrug", true);
                }
            }
        }
        else if (weapon.isPyroCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
            {
                if (playerStatsManager.currentMana >= playerInventoryManager.currentSpell.manaCost)
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);

                else
                {
                    playerAnimatorManager.PlayTargetAnimation("Shrug", true);
                }
            }
        }
    }

    private void PerformLTWeaponArt(bool isTwoHanding)
    {
        if (playerManager.isInteracting) return;

        if (isTwoHanding)
        {

        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(playerInventoryManager.leftWeapon.weapon_art, true);
        }
    }

    public void AttemptBackStabOrRipost()
    {
        if (playerStatsManager.currentStamina <= 0) return;

        RaycastHit hit;

        if (Physics.Raycast(inputHandler.criticalAttackRaycastStartPoint.position, transform.TransformDirection(Vector3.forward),
            out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null)
            {
                //Check for team mate id
                playerManager.transform.position = enemyCharacterManager.backStabCollider.CriticalDamageStandingPosition.position;
                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                //if (rightWeapon != null)
                //{
                int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;
                //}
                //else
                //{
                //    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * playerInventory.rightWeapon.baseDamage;
                //    enemyCharacterManager.pendingCriticalDamage = criticalDamage;
                //}

                playerAnimatorManager.PlayTargetAnimation("BackStab", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("BackStabbed", true);
            }
        }

        else if (Physics.Raycast(inputHandler.criticalAttackRaycastStartPoint.position, transform.TransformDirection(Vector3.forward),
            out hit, 0.7f, ripostLayer))
        {
            //Check for team mate id
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null && enemyCharacterManager.canBeReposted && enemyCharacterManager.isRepostableCharacter)
            {
                playerManager.transform.position = enemyCharacterManager.ripostCollider.CriticalDamageStandingPosition.position;

                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position; //Rotate player toward to enemy
                rotationDirection.y = 0;
                rotationDirection.Normalize();

                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                playerAnimatorManager.PlayTargetAnimation("Ripost", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
            }
        }
    }

    private void SuccessfulyCastSpell()
    {
        playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager);
        playerAnimatorManager.animator.SetBool("isFiringSpell", true);
    }

    #endregion

    #region Defense Actions

    void PerformLBBlockAction()
    {
        if (playerManager.isInteracting) return;

        if (playerManager.isBlocking) return;

        playerAnimatorManager.PlayTargetAnimation("Block_Start", false, true);
        playerEquipmentManager.OpenBlockingCollider();
        playerManager.isBlocking = true;
    }

    #endregion
}
