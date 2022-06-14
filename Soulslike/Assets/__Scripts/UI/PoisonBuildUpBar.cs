using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoisonBuildUpBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] PlayerStatsManager playerStats;

    private void Start()
    {
        slider = GetComponent<Slider>();

        slider.maxValue = 100;
        slider.value = 0;
        gameObject.SetActive(false);
    }

    //private void Update()
    //{
    //    if (slider.value == playerStats.currentHealth) return;
    //    slider.value = Mathf.Lerp(slider.value, playerStats.currentHealth, 3f * Time.deltaTime);
    //}

    public void SetPoisonBuildUp(int currentPoisonBuildUp)
    {
        slider.value = currentPoisonBuildUp;
    }
}
