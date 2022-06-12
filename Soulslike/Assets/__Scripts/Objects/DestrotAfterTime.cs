using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestrotAfterTime : MonoBehaviour
{
    public float timeUntilDestroy = 2f;

    private void Awake()
    {
        Destroy(gameObject, timeUntilDestroy);
    }
}
