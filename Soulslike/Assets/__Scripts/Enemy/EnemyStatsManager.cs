using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : CharacterStatsManager
{
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyManager enemyManager;
    [SerializeField] UIEnemyHealthBar enemyHealthBar;
    [SerializeField] EnemyBossManager enemyBossManager;

    public bool isBoss;

    private void Awake()
    {
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        enemyManager = GetComponent<EnemyManager>();

        maxHealth = SetMaxLevelHalth();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        if (!isBoss)
            enemyHealthBar.SetMaxHealth(maxHealth);
    }

    public override void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else if (poiseResetTimer <= 0 && !enemyManager.isInteracting)
        {
            totalPoiseDefense = armorPoiseBonus;
        }
    }

    private int SetMaxLevelHalth()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation = "Damage_01")
    {
        if (isDead) return;

        base.TakeDamage(physicalDamage, fireDamage, damageAnimation = "Damage_01");

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

    public override void TakePoisonDamage(int damage)
    {
        if (isDead) return;

        base.TakePoisonDamage(damage);

        if (!isBoss)
            enemyHealthBar.SetHealth(currentHealth);

        else if (isBoss && enemyBossManager != null)
        {
            enemyBossManager.UpdateBossHealBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
        }
    }

    public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
        if (isDead) return;

        base.TakeDamageNoAnimation(physicalDamage, fireDamage);

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

    public void BreakGuard()
    {
        enemyAnimatorManager.PlayTargetAnimation("Break_Guard", true);
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
