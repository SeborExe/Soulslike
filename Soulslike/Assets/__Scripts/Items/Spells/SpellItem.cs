using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : Item
{
    public GameObject spellWarmUpFX;
    public GameObject spellCastFX;
    public string spellAnimation;

    [Header("Spell cost")]
    public int manaCost;

    [Header("Spell type")]
    public bool isFaithSpell;
    public bool isMagicSpell;
    public bool isPyroSpell;

    [Header("Spell Discription")]
    [TextArea] public string spellDiscription;

    public virtual void AttemptToCastSpell(PlayerAnimatorManager animationHandler, PlayerStats playerStats, WeaponSlotManager weaponSlot)
    {
        Debug.Log("You attempt to cast a spell");
    }

    public virtual void SuccessfullyCastSpell(PlayerAnimatorManager animationHandler, PlayerStats playerStats,
        CameraHandler cameraHandler, WeaponSlotManager weaponSlot)
    {
        Debug.Log("You successfuly cast a spell");
        playerStats.DeductManaPoints(manaCost);
    }
}
