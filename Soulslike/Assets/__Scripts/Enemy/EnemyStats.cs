using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Animator anim;
    CriticalDamageCollider backStabCollider;
    EnemyAnimatorManager enemyAnimatorManager;

    public int soulsAwardedOnDeath = 1;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();

        //staminaBar = FindObjectOfType<StaminaBar>();
        //manaBar = FindObjectOfType<ManaBar>();
    }

    private void Start()
    {
        maxHealth = SetMaxLevelHalth();
        currentHealth = maxHealth;
    }

    private int SetMaxLevelHalth()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        if (isDead) return;

        currentHealth -= damage;

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
