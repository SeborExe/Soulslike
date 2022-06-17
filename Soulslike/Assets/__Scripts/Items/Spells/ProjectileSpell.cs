using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile Spell")]
public class ProjectileSpell : SpellItem
{
    [Header("Spell stats")]
    public float baseDamage;

    [Header("Spell physics")]
    public float projectileForwardVelocity;
    public float projcetileUpwardVelocity;
    public float projectileMass;
    public bool isEffectedByGravity;
    Rigidbody rigidbody;

    public override void AttemptToCastSpell(PlayerAnimatorManager animationHandler, PlayerStatsManager playerStats, PlayerWeaponSlotManager weaponSlot)
    {
        base.AttemptToCastSpell(animationHandler, playerStats, weaponSlot);
        GameObject InstantiateWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlot.rightHandSlot.transform);
        //InstantiateWarmUpSpellFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
        animationHandler.PlayTargetAnimation(spellAnimation, true);
    }

    public override void SuccessfullyCastSpell(PlayerAnimatorManager animationHandler,
        PlayerStatsManager playerStats, CameraHandler cameraHandler, PlayerWeaponSlotManager weaponSlot)
    {
        base.SuccessfullyCastSpell(animationHandler, playerStats, cameraHandler, weaponSlot);

        Vector3 startPos = playerStats.GetComponentInChildren<ProjectileStartingPos>().transform.position;
        GameObject InstantiateSpellFX = Instantiate(spellCastFX, startPos,
            cameraHandler.cameraPivotTransform.rotation);
        SpellDamageCollider spellDamageCollider = InstantiateSpellFX.GetComponent<SpellDamageCollider>();
        spellDamageCollider.teamIDNumber = playerStats.teamIDNumber;
        rigidbody = InstantiateSpellFX.GetComponent<Rigidbody>();
        //Spell DamageCollider

        if (cameraHandler.currentLockOnTarget != null)
        {
            InstantiateSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
        }
        else
        {
            InstantiateSpellFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, 
                playerStats.transform.eulerAngles.y, 0);
        }

        rigidbody.AddForce(InstantiateSpellFX.transform.forward * projectileForwardVelocity);
        rigidbody.AddForce(InstantiateSpellFX.transform.up * projcetileUpwardVelocity);
        rigidbody.useGravity = isEffectedByGravity;
        rigidbody.mass = projectileMass;
        InstantiateSpellFX.transform.parent = null;
    }
}
