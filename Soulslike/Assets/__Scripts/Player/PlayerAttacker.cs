using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    PlayerAnimatorManager animationHandler;
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerInventory playerInventory;
    InputHandler inputHandler;
    WeaponSlotManager weaponSlotManager;
    public string lastAttack;

    LayerMask backStabLayer = 1 << 10;
    LayerMask ripostLayer = 1 << 11;

    private void Awake()
    {
        animationHandler = GetComponent<PlayerAnimatorManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerStats = GetComponentInParent<PlayerStats>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        inputHandler = GetComponentInParent<InputHandler>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0)
            return;

        if (inputHandler.comboFlag)
        {
            animationHandler.anim.SetBool("canDoCombo", false);

            if (lastAttack == weapon.OH_Light_attack_01)
            {
                animationHandler.PlayTargetAnimation(weapon.OH_Light_attack_02, true);
            }
            else if (lastAttack == weapon.TH_Light_Attack_01)
            {
                animationHandler.PlayTargetAnimation(weapon.TH_Light_Attack_02, true);
            }
        }
    }
   
    public void HandleLightAttack(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0)
            return;

        weaponSlotManager.attackingWeapon = weapon;

        if (inputHandler.twoHandFlag)
        {
            animationHandler.PlayTargetAnimation(weapon.TH_Light_Attack_01, true);
            lastAttack = weapon.TH_Light_Attack_01;
        }
        else
        {
            animationHandler.PlayTargetAnimation(weapon.OH_Light_attack_01, true);
            lastAttack = weapon.OH_Light_attack_01;
        }
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0)
            return;

        weaponSlotManager.attackingWeapon = weapon;

        //if (inputHandler.twoHandFlag)
        //{

        //}
        //else
        //{
            animationHandler.PlayTargetAnimation(weapon.OH_Heavy_attack_01, true);
            lastAttack = weapon.OH_Heavy_attack_01;
        //}
    }

    #region Input Actions
    public void HandleRBAction()
    {
        if (playerInventory.rightWeapon.isMeleeWeapon)
        {
            PerformRBMeleeAction();
        }

        else if (playerInventory.rightWeapon.isSpellCaster ||
            playerInventory.rightWeapon.isFaithCaster ||
            playerInventory.rightWeapon.isPyroCaster)
        {
            PerformRBMagicAction(playerInventory.rightWeapon);
        }
    }

    public void HandleLTAction()
    {
        if (playerInventory.leftWeapon.isShieldWeapon)
        {
            PerformLTWeaponArt(inputHandler.twoHandFlag);
        }
        else if (playerInventory.leftWeapon.isMeleeWeapon)
        {

        }
    }

    #endregion

    #region Attack Actions
    private void PerformRBMeleeAction()
    {
        if (playerManager.canDoCombo)
        {
            inputHandler.comboFlag = true;
            HandleWeaponCombo(playerInventory.rightWeapon);
            inputHandler.comboFlag = false;
        }
        else
        {
            if (playerManager.isInteracting)
                return;

            if (playerManager.canDoCombo)
                return;

            animationHandler.anim.SetBool("isUsingRightHand", true);
            HandleLightAttack(playerInventory.rightWeapon);
        }
    }

    private void PerformRBMagicAction(WeaponItem weapon)
    {
        if (playerManager.isInteracting) return;

        if (weapon.isFaithCaster)
        {
            if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
            {
                if (playerStats.currentMana >= playerInventory.currentSpell.manaCost)
                    playerInventory.currentSpell.AttemptToCastSpell(animationHandler, playerStats);

                else
                {
                    animationHandler.PlayTargetAnimation("Shrug", true);
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
            animationHandler.PlayTargetAnimation(playerInventory.leftWeapon.weapon_art, true);
        }
    }

    public void AttemptBackStabOrRipost()
    {
        if (playerStats.currentStamina <= 0) return;

        RaycastHit hit;

        if (Physics.Raycast(inputHandler.criticalAttackRaycastStartPoint.position, transform.TransformDirection(Vector3.forward),
            out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

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
                int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;
                //}
                //else
                //{
                //    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * playerInventory.rightWeapon.baseDamage;
                //    enemyCharacterManager.pendingCriticalDamage = criticalDamage;
                //}

                animationHandler.PlayTargetAnimation("BackStab", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("BackStabbed", true);
            }
        }

        else if (Physics.Raycast(inputHandler.criticalAttackRaycastStartPoint.position, transform.TransformDirection(Vector3.forward),
            out hit, 0.7f, ripostLayer))
        {
            //Check for team mate id
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null && enemyCharacterManager.canBeReposted)
            {
                playerManager.transform.position = enemyCharacterManager.ripostCollider.CriticalDamageStandingPosition.position;

                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position; //Rotate player toward to enemy
                rotationDirection.y = 0;
                rotationDirection.Normalize();

                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                animationHandler.PlayTargetAnimation("Ripost", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
            }
        }
    }

    private void SuccessfulyCastSpell()
    {
        playerInventory.currentSpell.SuccessfullyCastSpell(animationHandler, playerStats);
    }

    #endregion
}
