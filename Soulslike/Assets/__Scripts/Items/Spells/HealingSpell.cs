using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Healing Spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;

    public override void AttemptToCastSpell(PlayerAnimatorManager animationHandler, PlayerStats playerStats, WeaponSlotManager weaponSlot)
    {
        base.AttemptToCastSpell(animationHandler, playerStats, weaponSlot);
        GameObject instantiatedWarpUpSpellFX = Instantiate(spellWarmUpFX, animationHandler.transform);
        animationHandler.PlayTargetAnimation(spellAnimation, true);
        Debug.Log("Attepting cast spell...");
    }

    public override void SuccessfullyCastSpell(PlayerAnimatorManager animationHandler, PlayerStats playerStats,
        CameraHandler cameraHandler, WeaponSlotManager weaponSlot)
    {
        base.SuccessfullyCastSpell(animationHandler, playerStats, cameraHandler, weaponSlot);
        GameObject instantiatedSpellFX = Instantiate(spellCastFX, animationHandler.transform);
        playerStats.HealPlayer(healAmount);
        Debug.Log("Spell cast succesfull...");
    }
}
