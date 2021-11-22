using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public abstract class Starship : MonoBehaviour
{
    // Polymorphic variables to be used by children classes
    [SerializeField] protected GameObject missilePrefab;
    [SerializeField] protected GameObject shield;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected HealthBar healthBar;

    [SerializeField] protected float speed;
    [SerializeField] protected float rotateSpeed;

    [SerializeField] protected int maxHealth;
    protected HealthSystem healthSystem;

    protected Rigidbody rb;
    protected Vector3 moveDirection;

    protected bool isInRange;
    protected bool isShieldActive;
    public bool shieldDown;

    [SerializeField] protected ParticleSystem laserHullParticle;
    [SerializeField] protected ParticleSystem laserShieldParticle;
    [SerializeField] protected ParticleSystem missileHullParticle;
    [SerializeField] protected GameObject deathParticle;

    [SerializeField] protected Transform vfxTransform;

    [SerializeField] protected AudioClip hullHitSound;
    [SerializeField] protected AudioClip missileHitSound;
    [SerializeField] protected AudioClip deathSound;
    protected AudioSource starshipAudio;

    protected virtual void Awake() // on script load initialize health system using starship's max health
    {
        isShieldActive = false;
        healthSystem = new HealthSystem(maxHealth);

        starshipAudio = GetComponent<AudioSource>();
        starshipAudio.volume = SoundManager.Instance.sfxVolume;
    }
    protected virtual void Start() // on start initialize health bar using starship's health system
    {
        rb = GetComponent<Rigidbody>();
        healthBar.SetUp(healthSystem);
    }
    private void FixedUpdate() // rotates ship to look direction, moves ship, checks to see if shield is active
    {
        RotateShip(GetLookDirection()); // ABSTRACTION
        MoveShip(); // ABSTRACTION
        ShieldActivation(); // ABSTRACTION
    }
    private void ShieldActivation() // sets shield object active only when shield bool is true
    {
        if (!shieldDown && isShieldActive && !shield.activeInHierarchy)
        {
            shield.SetActive(isShieldActive);
        }
        else if (shieldDown || (!isShieldActive && shield.activeInHierarchy))
        {
            shield.SetActive(isShieldActive);
        }
    }
    protected abstract Vector3 GetLookDirection(); // POLYMORPHISM
    private void RotateShip(Vector3 lookDirection) // Rotates starship to face the lookDirection vector
    {
        if (lookDirection != Vector3.zero)
        {
            Quaternion rotateShip = Quaternion.LookRotation(lookDirection);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotateShip, rotateSpeed));
        }
    }
    protected virtual void MoveShip() // moves starship in movement direction at a rate determined by starship's speed
    {
        rb.AddRelativeForce(moveDirection * speed);
    }
    protected void ShootPooledLaser() // takes a laser from the pool, places it at the starship's shoot point, sets lasers velocity and starts laser life timer
    {
        GameObject laser = LaserPool.SharedInstance.GetPooledObject();
        if (laser != null)
        {
            laser.transform.SetPositionAndRotation(shootPoint.position, transform.rotation);
            laser.SetActive(true);
            laser.GetComponent<MoveLaser>().StartLaserTimer();
        }
    }
    protected virtual void CheckDeath() // destroys starship if health is 0
    {
        if (healthSystem.GetHealth() <= 0) Destroy(gameObject);
    }
    protected virtual void OnTriggerEnter(Collider other) // checks to see if laser or missile hit starship
    {
        if (other.CompareTag("Laser"))
        {
            starshipAudio.PlayOneShot(hullHitSound, 0.1f);
            vfxTransform.localPosition = transform.InverseTransformPoint(other.transform.position);
            vfxTransform.localPosition = new Vector3(vfxTransform.localPosition.x, vfxTransform.localPosition.y + 5, vfxTransform.localPosition.z);
            laserHullParticle.Play();
            LaserHit(other);
            
        }
        else if (other.CompareTag("Missile"))
        {
            starshipAudio.PlayOneShot(missileHitSound, 0.1f);
            vfxTransform.localPosition = transform.InverseTransformPoint(other.transform.position);
            vfxTransform.localPosition = new Vector3(vfxTransform.localPosition.x, vfxTransform.localPosition.y + 5, vfxTransform.localPosition.z);
            missileHullParticle.Play();
            MissileHit(other);            
        }
    }
    protected void LaserHit(Collider laser) // does damage to starship's health system based on laser damage
    {
        MoveLaser moveLaser = laser.GetComponent<MoveLaser>();
        healthSystem.Damage(moveLaser.LaserDamage);
        ReturnToPool(laser);
        CheckDeath();
    }
    protected void MissileHit(Collider missile) // does damage to starship's health system based on missile damage
    {
        HomingMissile homingMissile = missile.GetComponent<HomingMissile>();
        healthSystem.Damage(homingMissile.MissileDamage);
        Destroy(missile.gameObject);
        CheckDeath();
    }
    protected void ReturnToPool(Collider other) // zero's velocity and disables object in active scene, returning the object to the pool
    {
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        other.gameObject.SetActive(false);
    }
}
