using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    [SerializeField] HealthBar healthBar;
    [SerializeField] StaminaBar staminaBar;
    [SerializeField] ManaBar manaBar;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerManager playerManager;

    [Header("Stamina")]
    [SerializeField] float staminaRegenerationAmount = 25;
    float staminaRegenerationTimer = 0;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }

    private void Start()
    {
        maxHealth = SetMaxLevelHalth();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        maxStamina = SetMaxStamina();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);

        maxMana = SetMaxMana();
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
    }

    public override void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else if (poiseResetTimer <= 0 && !playerManager.isInteracting)
        {
            totalPoiseDefense = armorPoiseBonus;
        }
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

    private float SetMaxMana()
    {
        maxMana = manaLevel * 10;
        return maxMana;
    }

    public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
    {
        if (isDead) return;

        if (playerManager.isInvulnerable) return;

        base.TakeDamage(damage, damageAnimation = "Damage_01");

        //healthBar.SetCurrentHealth(currentHealth);
        playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
        }
    }

    public override void TakePoisonDamage(int damage)
    {
        if (isDead) return;

        base.TakePoisonDamage(damage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
        }
    }

    public override void TakeDamageNoAnimation(int damage)
    {
        base.TakeDamageNoAnimation(damage);
    }

    public void TakeStaminaDamage(int damage)
    {
        currentStamina -= damage;

        //staminaBar.SetCurrentStamina(currentStamina);
    }

    public void RegenerateStamina()
    {
        if (playerManager.isInteracting)
            staminaRegenerationTimer = 0;

        else
        {
            staminaRegenerationTimer += Time.deltaTime;

            if (currentStamina < maxStamina && staminaRegenerationTimer > 1f)
            {
                currentStamina += staminaRegenerationAmount * Time.deltaTime;
                //staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
            }
        }

    }

    public void HealPlayer(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        //healthBar.SetCurrentHealth(currentHealth);
    }
  
    public void DeductManaPoints(int mana)
    {
        currentMana -= mana;

        if (currentMana < 0)
        {
            currentMana = 0;
        }

        //manaBar.SetCurrentMana(currentMana);
    }

    public void RestoreMana(int amount)
    {
        currentMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
    }

    public void AddSouls(int souls)
    {
        soulCount += souls;
    }
}
