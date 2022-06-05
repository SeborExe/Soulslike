using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Idle Animations")]
    public string Right_Hand_Idle;
    public string Left_Hand_Idle;
    public string Two_Hand_Idle;

    [Header("One Hand attack animations")]
    public string OH_Light_attack_01;//One hand light attack one
    public string OH_Light_attack_02;
    public string OH_Heavy_attack_01;
    public string TH_Light_Attack_01;
    public string TH_Light_Attack_02;

    [Header("Stamin Costs")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    [Header("Weapon type")]
    public bool isSpellCaster;
    public bool isFaithCaster;
    public bool isPyroCaster;
    public bool isMeleeWeapon;
    public bool isShieldWeapon;

    [Header("Weapon Art")]
    public string weapon_art;

    [Header("Damage")]
    public int baseDamage = 25;
    public int criticalDamageMultiplier = 4;

    [Header("Absorption")]
    public float physicalDamageAbsorption;
}
