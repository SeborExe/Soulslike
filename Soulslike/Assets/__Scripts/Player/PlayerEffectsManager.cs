using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : MonoBehaviour
{
    public GameObject currentParticleFX; //current effects that are affecting the player, such as poison
    PlayerStats playerStats;
    WeaponSlotManager weaponSlotManager;

    public int amoutToBeHealed;
    public GameObject instantiateFXModel;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    public void HealPlayerFromEffect()
    {
        playerStats.HealPlayer(amoutToBeHealed);
        GameObject healParticles = Instantiate(currentParticleFX, playerStats.transform);
        Destroy(instantiateFXModel.gameObject);
        weaponSlotManager.LoadBothWeaponsOnSlot();
    }
}
