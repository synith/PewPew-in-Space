using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private HealthBar uiHealthBar;
    [SerializeField] private PlayerController playerController;
    private void Start()
    {
        uiHealthBar.SetUp(playerController.GetHealthSystem());
    }
}
