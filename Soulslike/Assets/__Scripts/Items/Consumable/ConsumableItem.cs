using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    [Header("Item Quantity")]
    public int maxItemAmout;
    public int currentItemAmout;

    [Header("Item Model")]
    public GameObject itemModel;

    [Header("Animations")]
    public string consumableAnimation;
    public bool isInteracting;

    public virtual void AttemptToConsumableItem(PlayerAnimatorManager playerAnimatorManager,
        PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        if (currentItemAmout > 0)
        {
            playerAnimatorManager.PlayTargetAnimation(consumableAnimation, isInteracting, true);
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation("Flask_Empty", true);
        }
    }
}
