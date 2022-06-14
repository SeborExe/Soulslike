using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumable/Clump")]
public class ClumpConsumableItem : ConsumableItem
{
    [Header("Recovery effects")]
    public GameObject clumpFX;

    [Header("CureFX")]
    public bool curePoison;
    //Cure bleeding
    //Cure curse etc...

    public override void AttemptToConsumableItem(PlayerAnimatorManager playerAnimatorManager,
        PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        base.AttemptToConsumableItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
        GameObject clump = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
        playerEffectsManager.currentParticleFX = clumpFX;
        playerEffectsManager.instantiateFXModel = clump;

        if (curePoison)
        {
            playerEffectsManager.poisonBuildUp = 0;
            playerEffectsManager.poisonAmount = playerEffectsManager.defaultPoisonAmount;
            playerEffectsManager.isPoisoned = false;

            if (playerEffectsManager.currentPoisonParticleFX != null)
            {
                Destroy(playerEffectsManager.currentPoisonParticleFX);
            }
        }

        weaponSlotManager.rightHandSlot.UnloadWeapon();
    }
}
