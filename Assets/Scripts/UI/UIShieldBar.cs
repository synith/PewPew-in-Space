using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShieldBar : MonoBehaviour
{
    [SerializeField] private HealthBar uiShieldBar;
    [SerializeField] private Shield playerShield;
    private void Start()
    {
        uiShieldBar.SetUp(playerShield.GetHealthSystem());
    }
}
