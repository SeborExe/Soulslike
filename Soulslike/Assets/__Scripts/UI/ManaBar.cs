using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] PlayerStatsManager playerStats;

    private void Update()
    {
        if (slider.value == playerStats.currentMana) return;
        slider.value = Mathf.Lerp(slider.value, playerStats.currentMana, 3f * Time.deltaTime);
    }

    public void SetMaxMana(float maxMana)
    {
        slider.maxValue = maxMana;
        slider.value = maxMana;
    }

    public void SetCurrentMana(float currentMana)
    {
        slider.value = currentMana;
    }
}
