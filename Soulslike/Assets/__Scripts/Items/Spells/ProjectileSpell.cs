using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile Spell")]
public class ProjectileSpell : SpellItem
{
    public float baseDamage;
    public float projectileVelocity;
    Rigidbody rigidbody;

    public override void AttemptToCastSpell(PlayerAnimatorManager animationHandler, PlayerStats playerStats, WeaponSlotManager weaponSlot)
    {
        base.AttemptToCastSpell(animationHandler, playerStats, weaponSlot);
        GameObject InstantiateWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlot.rightHandSlot.transform);
        //InstantiateWarmUpSpellFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
        animationHandler.PlayTargetAnimation(spellAnimation, true);
    }

    public override void SuccessfullyCastSpell(PlayerAnimatorManager animationHandler, PlayerStats playerStats)
    {
        base.SuccessfullyCastSpell(animationHandler, playerStats);
    }
}
