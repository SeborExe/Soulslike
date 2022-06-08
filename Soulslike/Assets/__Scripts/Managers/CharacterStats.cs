using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
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

    public bool isDead;

    [Header("Armor reduction")]
    public float physicalDamageAbsorbtionHead;
    public float physicalDamageAbsorbtionTorso;
    public float physicalDamageAbsorbtionPants;
    public float physicalDamageAbsorbtionGloves;
    public float physicalDamageAbsorbtionBoots;
    //Same for fire,magic etc.

    public virtual void TakeDamage(int physicalDamage, string damageAnimation = "Damage_01")
    {
        if (isDead) return;

        float totalPhysicalDamageAbsorbtion = 1 -
            (1 - physicalDamageAbsorbtionHead / 100) *
            (1 - physicalDamageAbsorbtionTorso / 100) *
            (1 - physicalDamageAbsorbtionPants / 100) *
            (1 - physicalDamageAbsorbtionGloves / 100) *
            (1 - physicalDamageAbsorbtionBoots / 100);

        physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorbtion));

        float finalDamage = physicalDamage; //+ fire damage + magic damage etc.

        currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }
}
