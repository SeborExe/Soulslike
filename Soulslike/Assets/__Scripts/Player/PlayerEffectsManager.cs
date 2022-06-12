using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : MonoBehaviour
{
    public GameObject currentParticleFX; //current effects that are affecting the player, such as poison
    PlayerStatsManager playerStatsManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;

    public int amoutToBeHealed;
    public GameObject instantiateFXModel;

    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
    }

    public void HealPlayerFromEffect()
    {
        playerStatsManager.HealPlayer(amoutToBeHealed);
        GameObject healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
        Destroy(instantiateFXModel.gameObject);
        playerWeaponSlotManager.LoadBothWeaponsOnSlot();
    }
}
