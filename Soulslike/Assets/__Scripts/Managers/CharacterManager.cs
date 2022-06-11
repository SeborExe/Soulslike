using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
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

    [Header("Movement Flags")]
    public bool isRotatingWithRootMotion;
    public bool canRotate;

    [Header("Spells")]
    public bool isFiringSpell;

    public int pendingCriticalDamage;
}