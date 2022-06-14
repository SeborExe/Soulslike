using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    public GameObject currentParticleFX; //current effects that are affecting the player, such as poison
    PlayerStatsManager playerStatsManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;

    [SerializeField] PoisonBuildUpBar poisonBuildUpBar;
    [SerializeField] PoisonAmountBar poisonAmountBar;

    public int amoutToBeHealed;
    public GameObject instantiateFXModel;

    protected override void Awake()
    {
        base.Awake();
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

    protected override void HandlePoisonBuildUp()
    {
        if (poisonBuildUp <= 0)
        {
            poisonBuildUpBar.gameObject.SetActive(false);
        }
        else
        {
            poisonBuildUpBar.gameObject.SetActive(true);
        }

        base.HandlePoisonBuildUp();
        poisonBuildUpBar.SetPoisonBuildUp(Mathf.RoundToInt(poisonBuildUp));
    }

    protected override void HadleIsPoisonedEffect()
    {
        if (!isPoisoned)
        {
            poisonAmountBar.gameObject.SetActive(false);
        }
        else
        {
            poisonAmountBar.gameObject.SetActive(true);
        }

        base.HadleIsPoisonedEffect();
        poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
    }
}
