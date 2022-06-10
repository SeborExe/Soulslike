using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    public UIBossHealthBar bossHealthBar;
    public WorldEventManager worldEventManager;
    public FogWall fogWall;

    public string bossName;
    EnemyStats enemyStats;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        bossHealthBar.SetBossName(bossName);
        bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
    }

    public void UpdateBossHealBar(int currentHealth)
    {
        bossHealthBar.SetBossCurrentHealth(currentHealth);
    }
}
