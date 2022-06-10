using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Animator anim;
    CriticalDamageCollider backStabCollider;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyManager enemyManager;
    [SerializeField] UIEnemyHealthBar enemyHealthBar;

    public int soulsAwardedOnDeath = 1;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyManager = GetComponentInParent<EnemyManager>();

        maxHealth = SetMaxLevelHalth();
        currentHealth = maxHealth;
    }

    private void Start()
    {
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
        enemyHealthBar.SetHealth(currentHealth);

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
        }
    }

    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth -= damage;
        enemyHealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            HandleDeath();
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
}
