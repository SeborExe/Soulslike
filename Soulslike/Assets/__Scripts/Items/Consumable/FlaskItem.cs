using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumable/Flask")]
public class FlaskItem : ConsumableItem
{
    [Header("Flash type")]
    public bool estustFlask;
    public bool ashenFlask;

    [Header("Recovery Amount")]
    public int healRecoveryAmount;
    public int manaRecoveryAmount;

    [Header("Recovery effects")]
    public GameObject recoveryFX;

    public override void AttemptToConsumableItem(PlayerAnimatorManager playerAnimatorManager,
        WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        base.AttemptToConsumableItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
        GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
        playerEffectsManager.currentParticleFX = recoveryFX;
        playerEffectsManager.amoutToBeHealed = healRecoveryAmount;
        playerEffectsManager.instantiateFXModel = flask;
        weaponSlotManager.rightHandSlot.UnloadWeapon();
    }
}
