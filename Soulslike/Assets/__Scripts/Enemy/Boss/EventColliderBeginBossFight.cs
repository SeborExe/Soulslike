using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventColliderBeginBossFight : MonoBehaviour
{
    [SerializeField] WorldEventManager worldEventManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            worldEventManager.ActiveBossFight();
            Destroy(gameObject);
        }
    }
}
