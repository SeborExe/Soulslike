using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public int staminaLevel = 10;
    public float maxStamina;
    public float currentStamina;

    public int manaLevel = 10;
    public float maxMana;
    public float currentMana;

    public int soulCount = 0;
    public int soulsAwardedOnDeath = 1;

    public bool isDead;

    [Header("Poise")]
    public float totalPoiseDefense; //The total poise during damage calculation
    public float offensivePoiseBonus;
    public float armorPoiseBonus;
    public float totalPoiseResetTime = 15f;
    public float poiseResetTimer = 0f;

    [Header("Armor PHYSICAL reduction")]
    public float physicalDamageAbsorbtionHead;
    public float physicalDamageAbsorbtionTorso;
    public float physicalDamageAbsorbtionPants;
    public float physicalDamageAbsorbtionGloves;
    public float physicalDamageAbsorbtionBoots;

    [Header("Armor FIRE reduction")]
    public float fireDamageAbsorbtionHead;
    public float fireDamageAbsorbtionTorso;
    public float fireDamageAbsorbtionPants;
    public float fireDamageAbsorbtionGloves;
    public float fireDamageAbsorbtionBoots;


    private void Start()
    {
        totalPoiseDefense = armorPoiseBonus;
    }

    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }

    public virtual void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation = "Damage_01")
    {
        if (isDead) return;

        //Physic Damage absorbtion
        float totalPhysicalDamageAbsorbtion = 1 -
            (1 - physicalDamageAbsorbtionHead / 100) *
            (1 - physicalDamageAbsorbtionTorso / 100) *
            (1 - physicalDamageAbsorbtionPants / 100) *
            (1 - physicalDamageAbsorbtionGloves / 100) *
            (1 - physicalDamageAbsorbtionBoots / 100);

        physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorbtion));

        //Fire Damage absorbtion
        float totalFireDamageAbsorbtion = 1 -
            (1 - fireDamageAbsorbtionHead / 100) *
            (1 - fireDamageAbsorbtionTorso / 100) *
            (1 - fireDamageAbsorbtionPants / 100) *
            (1 - fireDamageAbsorbtionGloves / 100) *
            (1 - fireDamageAbsorbtionBoots / 100);

        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorbtion));

        float finalDamage = physicalDamage + fireDamage; //+ fire damage + magic damage etc.

        currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
        if (isDead) return;

        //Physic Damage absorbtion
        float totalPhysicalDamageAbsorbtion = 1 -
            (1 - physicalDamageAbsorbtionHead / 100) *
            (1 - physicalDamageAbsorbtionTorso / 100) *
            (1 - physicalDamageAbsorbtionPants / 100) *
            (1 - physicalDamageAbsorbtionGloves / 100) *
            (1 - physicalDamageAbsorbtionBoots / 100);

        physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorbtion));

        //Fire Damage absorbtion
        float totalFireDamageAbsorbtion = 1 -
            (1 - fireDamageAbsorbtionHead / 100) *
            (1 - fireDamageAbsorbtionTorso / 100) *
            (1 - fireDamageAbsorbtionPants / 100) *
            (1 - fireDamageAbsorbtionGloves / 100) *
            (1 - fireDamageAbsorbtionBoots / 100);

        fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorbtion));

        float finalDamage = physicalDamage + fireDamage; //+ fire damage + magic damage etc.

        currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public virtual void TakePoisonDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public virtual void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else
        {
            totalPoiseDefense = armorPoiseBonus;
        }
    }
}
