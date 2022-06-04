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

    public int pendingCriticalDamage;
}
