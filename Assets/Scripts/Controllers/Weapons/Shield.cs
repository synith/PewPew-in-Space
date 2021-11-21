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

    [SerializeField] private AudioClip shieldHitClip;
    [SerializeField] private AudioClip shieldDownClip;
    [SerializeField] private AudioClip shieldRegenClip;
    private AudioSource shieldAudioSource;

    private Starship starship;

    private void Awake() // initialize variables and begin shield regen feature
    {
        shieldMesh = GetComponent<MeshRenderer>();
        shieldAudioSource = GetComponent<AudioSource>();
        starship = GetComponentInParent<Starship>();

        starship.shieldDown = false;

        healthSystem = new HealthSystem(maxShieldHealth);

        shieldAudioSource.volume = SoundManager.Instance.sfxVolume;
        shieldColor = shieldMesh.material.color;
        shieldAlpha = shieldColor.a;
        StartCoroutine(nameof(ShieldRegen));
    }
    private void OnTriggerEnter(Collider other) // check if a laser hit the shield (missiles go through shield)
    {
        if (other.CompareTag("Laser") && starship.gameObject.CompareTag("Shield"))
            LaserHit(other);
    }
    private void LaserHit(Collider other) // does damage to health system based on laser damage
    {
        MoveLaser moveLaser = other.GetComponent<MoveLaser>();
        healthSystem.Damage(moveLaser.LaserDamage);

        shieldAudioSource.PlayOneShot(shieldHitClip, 0.1f);

        SetShieldTransparency();
        CheckDeath();
    }
    private void SetShieldTransparency() // set the shield color's alpha value to a percentage of its original/full value based on shield's hp percentage
    {
        float shieldPercent;
        float currentHealth = healthSystem.GetHealth();
        float maxShieldHealthFloat = maxShieldHealth;

        shieldPercent = currentHealth / maxShieldHealthFloat;

        shieldNewAlpha = shieldAlpha * shieldPercent;
        shieldMesh.material.color = new Color(shieldColor.r, shieldColor.g, shieldColor.b, shieldNewAlpha);
    }
    private void CheckDeath() // if shield has 0 hp, set the shield as down and tag the player so they can be damaged again, start the shield regen coroutine
    {
        if (healthSystem.GetHealth() <= 0)
        {
            shieldAudioSource.PlayOneShot(shieldDownClip, 0.1f);
            starship.shieldDown = true;            
            starship.gameObject.tag = "Player";
            StartCoroutine(nameof(ShieldRegen));
        }
    }
    public IEnumerator ShieldRegen() // after a set amount of time heal the shield to full, set the shield as up and tag the player so the shield blocks lasers for them
    {
        yield return new WaitForSeconds(shieldRegenTime);        

        if(gameObject.activeInHierarchy && starship.shieldDown)
        {
            healthSystem.Heal(maxShieldHealth);
            shieldAudioSource.PlayOneShot(shieldRegenClip, 0.1f);
            starship.gameObject.tag = "Shield";
            SetShieldTransparency();
            starship.shieldDown = false;
        }            
    }
    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}
