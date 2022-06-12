using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] PlayerStatsManager playerStats;

    private void Update()
    {
        if (slider.value == playerStats.currentHealth) return;
        slider.value = Mathf.Lerp(slider.value, playerStats.currentHealth, 3f * Time.deltaTime);
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetCurrentHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }
}
