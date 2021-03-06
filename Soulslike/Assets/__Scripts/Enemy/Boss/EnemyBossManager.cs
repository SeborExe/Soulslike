using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    public UIBossHealthBar bossHealthBar;
    public WorldEventManager worldEventManager;
    public FogWall fogWall;

    public string bossName;
    EnemyStatsManager enemyStats;
    EnemyAnimatorManager enemyAnimatorManager;
    BossCombatStanceState bossCombatStanceState;

    [Header("Second Phase FX")]
    public GameObject particleFX;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStatsManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }

    private void Start()
    {
        bossHealthBar.SetBossName(bossName);
        bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
    }

    public void UpdateBossHealBar(int currentHealth, int maxHealth)
    {
        bossHealthBar.SetBossCurrentHealth(currentHealth);

        if (currentHealth <= (maxHealth / 2) && !bossCombatStanceState.hasPhaseShifted)
        {
            ShiftToSecondPhase();
        }
    }

    public void ShiftToSecondPhase()
    {
        enemyAnimatorManager.animator.SetBool("isInvulnerable", true);
        enemyAnimatorManager.animator.SetBool("isPhaseShifting", true);
        enemyAnimatorManager.PlayTargetAnimation("Phase_Shift", true);
        bossCombatStanceState.hasPhaseShifted = true;
        Debug.Log("Phase shift");
    }


}
