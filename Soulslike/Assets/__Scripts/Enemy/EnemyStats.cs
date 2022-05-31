using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Animator anim;

    private void Awake()
    {
        //playerManager = GetComponent<PlayerManager>();
        anim = GetComponentInChildren<Animator>();

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

    public void TakeDamage(int damage)
    {
        //if (playerManager.isInvulnerable) return;
        if (isDead) return;

        currentHealth -= damage;

        anim.Play("Damage_01");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            anim.Play("Dead_01");
            isDead = true;
        }
    }
}
