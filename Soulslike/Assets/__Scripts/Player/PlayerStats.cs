using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public int staminaLevel = 10;
    public float maxStamina;
    public float currentStamina;

    public bool isDead;

    [SerializeField] HealthBar healthBar;
    [SerializeField] StaminaBar staminaBar;
    PlayerAnimatorManager animationHandler;

    private void Awake()
    {
        //playerManager = GetComponent<PlayerManager>();
        animationHandler = GetComponentInChildren<PlayerAnimatorManager>();

        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        //manaBar = FindObjectOfType<ManaBar>();
    }

    private void Start()
    {
        maxHealth = SetMaxLevelHalth();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        maxStamina = SetMaxStamina();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    private int SetMaxLevelHalth()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    private float SetMaxStamina()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
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

    public void TakeStaminaDamage(int damage)
    {
        currentStamina -= damage;

        staminaBar.SetCurrentStamina(currentStamina);
    }
}
