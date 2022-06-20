using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{
    public override void GrantWeaponAttackingPoiseBonus()
    {
        characterStatsManager.totalPoiseDefense += characterStatsManager.offensivePoiseBonus;
    }

    public override void ResetWeaponAttackingPoiseBonus()
    {
        characterStatsManager.totalPoiseDefense = characterStatsManager.armorPoiseBonus;
    }
}
