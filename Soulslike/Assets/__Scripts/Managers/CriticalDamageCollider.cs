using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalDamageCollider : MonoBehaviour
{
    public Transform CriticalDamageStandingPosition;

    public void DeactivateBackStabCollider()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.enabled = false;
    }
}
