using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour // health bar using slider that sets its values according to health system values
{
    private Slider slider;
    private HealthSystem healthSystem;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    public void SetUp(HealthSystem healthSystem) // initialize health bar values using health system values
    {
        this.healthSystem = healthSystem;

        SetMaxHealth(healthSystem.GetHealth());
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged; // links health system's OnHealthChanged event to HealthSystem_OnHealthChanged method
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e) // everytime healthchanged event is invoked call the SetHealth method
    {
        SetHealth(healthSystem.GetHealth());
    }

    public void SetMaxHealth(int maxHealth) // set the max value of the slider to the max health of the health system
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }
    public void SetHealth(int health) // set the health of the healthbar based on current health of the health system
    {
        slider.value = health;
    }

}
