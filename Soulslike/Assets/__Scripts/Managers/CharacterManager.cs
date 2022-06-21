using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    CharacterAnimatorManager characterAnimatorManager;
    CharacterWeaponSlotManager characterWeaponSlotManager;

    [Header("Lock on transform")]
    public Transform lockOnTransform;

    [Header("Combat colliders")]
    public CriticalDamageCollider backStabCollider;
    public CriticalDamageCollider ripostCollider;

    [Header("Combat flag")]
    public bool canBeReposted;
    public bool isParrying;
    public bool canBeParried;
    public bool isRepostableCharacter;
    public bool isBlocking;
    public bool isInvulnerable;
    public bool canDoCombo;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;
    public bool isTwoHandWeapon;

    [Header("Movement Flags")]
    public bool isRotatingWithRootMotion;
    public bool canRotate;
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;

    [Header("Spells")]
    public bool isFiringSpell;

    public int pendingCriticalDamage;

    protected virtual void Awake()
    {
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
    }

    protected virtual void FixedUpdate()
    {
        characterAnimatorManager.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget, characterWeaponSlotManager.leftHandIKTarget, isTwoHandWeapon);
    }
}
