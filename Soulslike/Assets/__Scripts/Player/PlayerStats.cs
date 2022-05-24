using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public bool isDead;

    [SerializeField] HealthBar healthBar;
    PlayerAnimatorManager animationHandler;

    private void Awake()
    {
        //playerManager = GetComponent<PlayerManager>();
        animationHandler = GetComponentInChildren<PlayerAnimatorManager>();

        healthBar = FindObjectOfType<HealthBar>();
        //staminaBar = FindObjectOfType<StaminaBar>();
        //manaBar = FindObjectOfType<ManaBar>();
    }

    private void Start()
    {
        maxHealth = SetMaxLevelHalth();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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

        healthBar.SetCurrentHealth(currentHealth);
        animationHandler.PlayTargetAnimation("Damage_01", true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animationHandler.PlayTargetAnimation("Dead_01", true);
            isDead = true;
        }
    }
}
