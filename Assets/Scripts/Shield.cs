using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private MeshRenderer shieldMesh;
    private Color shieldColor;
    private float shieldAlpha;
    private float shieldNewAlpha;
    private HealthSystem healthSystem;
    [SerializeField] private int maxShieldHealth = 30;
    [SerializeField] private float shieldRegenTime = 5;

    private Starship starship;

    private void Awake()
    {
        shieldMesh = GetComponent<MeshRenderer>();
        starship = GetComponentInParent<Starship>();
        starship.shieldDown = false;
        healthSystem = new HealthSystem(maxShieldHealth);
        shieldColor = shieldMesh.material.color;
        shieldAlpha = shieldColor.a;
        StartCoroutine(nameof(ShieldRegen));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser") && starship.gameObject.CompareTag("Shield"))
            LaserHit(other);
    }
    protected void LaserHit(Collider other) // does damage to health system based on laser damage
    {
        MoveLaser moveLaser = other.GetComponent<MoveLaser>();
        healthSystem.Damage(moveLaser.LaserDamage);
        SetShieldTransparency();
        CheckDeath();
    }
    private void SetShieldTransparency()
    {
        float shieldPercent;
        float currentHealth = healthSystem.GetHealth();
        float maxShieldHealthFloat = maxShieldHealth;

        shieldPercent = currentHealth / maxShieldHealthFloat;

        shieldNewAlpha = shieldAlpha * shieldPercent;
        shieldMesh.material.color = new Color(shieldColor.r, shieldColor.g, shieldColor.b, shieldNewAlpha);
    }
    private void CheckDeath()
    {
        if (healthSystem.GetHealth() <= 0)
        {
            starship.shieldDown = true;            
            starship.gameObject.tag = "Player";
            StartCoroutine(nameof(ShieldRegen));
        }
    }
    public IEnumerator ShieldRegen()
    {
        yield return new WaitForSeconds(shieldRegenTime);
        healthSystem.Heal(maxShieldHealth);
        SetShieldTransparency();
        starship.shieldDown = false;
        starship.gameObject.tag = "Shield";
    }
}
