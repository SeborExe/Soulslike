using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    [SerializeField] UIBossHealthBar bossHealthBar;

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
}
