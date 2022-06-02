using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Lock on transform")]
    public Transform lockOnTransform;

    [Header("Combat colliders")]
    public BoxCollider backStabBoxCollider;
    public BackStabCollider backStabCollider;

    public int pendingCriticalDamage;
}
