using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyManager enemyManager;
    [SerializeField] UIEnemyHealthBar enemyHealthBar;
    [SerializeField] EnemyBossManager enemyBossManager;

    public int soulsAwardedOnDeath = 1;
    public bool isBoss;

    private void Awake()
    {
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyManager = GetComponent<EnemyManager>();

        maxHealth = SetMaxLevelHalth();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        if (!isBoss)
            enemyHealthBar.SetMaxHealth(maxHealth);
    }

    private int SetMaxLevelHalth()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        base.TakeDamage(damage, damageAnimation = "Damage_01");

        if (!isBoss)
            enemyHealthBar.SetHealth(currentHealth);

        else if (isBoss && enemyBossManager != null)
        {
            enemyBossManager.UpdateBossHealBar(currentHealth, maxHealth);
        }

        if (enemyManager.currentTarget == null)
        {
            enemyManager.maximumDetectionAngle = 360f;
            enemyManager.minimumDetectionAngle = -360f;
        }   

        enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

        if (currentHealth <= 0)
        {
            enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
            HandleDeath();

            if (isBoss)
            {
                HandleBossDeath();
            }
        }
    }

    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth -= damage;

        if (!isBoss)
            enemyHealthBar.SetHealth(currentHealth);

        else if (isBoss && enemyBossManager != null)
        {
            enemyBossManager.UpdateBossHealBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            HandleDeath();

            if (isBoss)
            {
                HandleBossDeath();
            }
        }
    }

    private void HandleDeath()
    {
        currentHealth = 0;
        isDead = true;
        BoxCollider[] criticalColliders = GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider collider in criticalColliders)
        {
            Destroy(collider);
        }
    }

    private void HandleBossDeath()
    {
        enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
        enemyBossManager.worldEventManager.bossHasBeenDefeded = true;
        enemyBossManager.bossHealthBar.SetHealthBarToInactive();
        enemyBossManager.fogWall.DesactiveFogWall();
    }
}
