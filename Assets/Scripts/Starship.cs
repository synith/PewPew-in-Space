using UnityEngine;
public abstract class Starship : MonoBehaviour
{
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

    protected virtual void Awake()
    {
        isShieldActive = false;
        healthSystem = new HealthSystem(maxHealth);
    }
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        healthBar.SetUp(healthSystem);
    }
    private void FixedUpdate()
    {
        RotateShip(GetLookDirection());
        MoveShip();
        ShieldActivation();
    }
    private void ShieldActivation()
    {
        if (isShieldActive && !shield.activeInHierarchy)
        {
            shield.SetActive(isShieldActive);
        }
        else if (!isShieldActive && shield.activeInHierarchy)
        {
            shield.SetActive(isShieldActive);
        }
    }
    protected abstract Vector3 GetLookDirection();
    private void RotateShip(Vector3 lookDirection)
    {
        if (lookDirection != Vector3.zero)
        {
            Quaternion rotateShip = Quaternion.LookRotation(lookDirection);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotateShip, rotateSpeed));
        }
    }
    protected virtual void MoveShip()
    {
        rb.AddRelativeForce(moveDirection * speed);
    }
    protected void ShootPooledLaser()
    {
        GameObject laser = LaserPool.SharedInstance.GetPooledObject();
        if (laser != null)
        {
            laser.transform.SetPositionAndRotation(shootPoint.position, transform.rotation);
            laser.SetActive(true);
            laser.GetComponent<MoveLaser>().StartLaserTimer();
        }
    }
    protected virtual void CheckDeath()
    {
        if (healthSystem.GetHealth() <= 0) Destroy(gameObject);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser"))
        {
            LaserHit(other);
        }
        else if (other.CompareTag("Missile"))
        {
            MissileHit(other);
        }
    }
    protected void LaserHit(Collider other)
    {
        MoveLaser moveLaser = other.GetComponent<MoveLaser>();
        healthSystem.Damage(moveLaser.LaserDamage);
        ReturnToPool(other);
        CheckDeath();
    }
    protected void MissileHit(Collider other)
    {
        HomingMissile homingMissile = other.GetComponent<HomingMissile>();
        healthSystem.Damage(homingMissile.MissileDamage);
        Destroy(other.gameObject);
        CheckDeath();
    }
    protected void ReturnToPool(Collider other)
    {
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        other.gameObject.SetActive(false);
    }
}
