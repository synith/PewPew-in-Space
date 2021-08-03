using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    private HealthSystem healthSystem;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    public void SetUp(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;

        SetMaxHealth(healthSystem.GetHealth());
        Debug.Log("Health Bar Initialized!");
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        SetHealth(healthSystem.GetHealth());
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }
    public void SetHealth(int health)
    {
        slider.value = health;
    }

}
