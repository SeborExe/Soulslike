using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Animator replacer")]
    public AnimatorOverrideController weaponController;
    public string offHandIdleAnimation = "Left_Arm_Idle";

    [Header("Weapon Type")]
    public WeaponType weaponType;

    [Header("Stamin Costs")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    [Header("Damage")]
    public int physicalDamage = 25;
    public int fireDamage = 0;
    public int criticalDamageMultiplier = 4;

    [Header("Poise")]
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Absorption")]
    public float physicalDamageAbsorption;
}
