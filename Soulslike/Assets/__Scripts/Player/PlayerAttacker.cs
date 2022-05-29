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

    //LayerMask backStabLayer = 1 << 12;

    private void Awake()
    {
        animationHandler = GetComponentInChildren<PlayerAnimatorManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerStats = GetComponentInParent<PlayerStats>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        inputHandler = GetComponentInParent<InputHandler>();
    }

    
    public void HandleWeaponCombo(WeaponItem weapon)
    {
        //if (playerStats.currentStamina <= 0)
        //    return;

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
        //if (playerStats.currentStamina <= 0)
        //    return;

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
        //if (playerStats.currentStamina <= 0)
        //    return;

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
}
