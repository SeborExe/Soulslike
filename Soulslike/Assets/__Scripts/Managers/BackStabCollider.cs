using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackStabCollider : MonoBehaviour
{
    public Transform backStabberStandPoint;

    public void DeactivateBackStabCollider()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.enabled = false;
    }
}
