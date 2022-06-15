using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumable/Bomb Item")]
public class BombConsumableItem : ConsumableItem
{
    [Header("Velocity")]
    public int upwardVelicoty = 50;
    public int forwardVelocity = 50;
    public int bombMass = 1;

    [Header("Live bomb Model")]
    public GameObject liveBombModel;

    [Header("Base Damage")]
    public int bombDamage = 200;

    public override void AttemptToConsumableItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        if (currentItemAmout > 0)
        {
            playerAnimatorManager.PlayTargetAnimation(consumableAnimation, true);
            GameObject bombModel = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform.position,
                Quaternion.identity, weaponSlotManager.rightHandSlot.transform);
            playerEffectsManager.instantiateFXModel = bombModel;
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation("Shrug", true);
        }
    }
}
