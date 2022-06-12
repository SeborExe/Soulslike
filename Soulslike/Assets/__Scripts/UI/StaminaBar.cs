using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] PlayerStatsManager playerStats;

    private void Update()
    {
        if (slider.value == playerStats.currentStamina) return;
        slider.value = Mathf.Lerp(slider.value, playerStats.currentStamina, 3f * Time.deltaTime);
    }

    public void SetMaxStamina(float maxStamina)
    {
        slider.maxValue = maxStamina;
        slider.value = maxStamina;
    }

    public void SetCurrentStamina(float currentStamina)
    {
        slider.value = currentStamina;
    }
}
