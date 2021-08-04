using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float checkDistanceSeconds;
    private Transform playerPosition;

    private HealthSystem healthSystem;

    private Vector3 moveDirection;
    private Rigidbody rb;

    private bool isInRange;
    private bool isTooClose;

    private float minRange = 80;
    private float closeRange = 40;

    [SerializeField] private int maxHealth;

    private void Awake()
    {
        FindPlayer();        
    }

    private void Start()
    {
        HealthBar healthBar = GetComponentInChildren<HealthBar>();
        healthSystem = new HealthSystem(maxHealth);

        rb = GetComponent<Rigidbody>();
        healthBar.SetUp(healthSystem);

        

        InvokeRepeating(nameof(CheckDistance), checkDistanceSeconds, checkDistanceSeconds);
        InvokeRepeating(nameof(ShootLaser), 0.5f, 0.5f);
    }
    private void FixedUpdate()
    {
        // ABSTRACTION
        RotateShip();
        MoveShip();
    }
    private void FindPlayer()
    {
        playerPosition = FindObjectOfType<PlayerController>().transform;
    }
    private void CheckDistance()
    {
        float dist = Vector3.Distance(playerPosition.position, transform.position);

        if (dist < minRange)
            isInRange = true;
        else
            isInRange = false;

        if (dist < closeRange)
            isTooClose = true;
        else
            isTooClose = false;
    }
    private void ShootPooledObject()
    {
        GameObject laser = LaserPool.SharedInstance.GetPooledObject();
        if (laser != null)
        {
            laser.transform.SetPositionAndRotation(shootPoint.position, transform.rotation);
            laser.SetActive(true);
            laser.GetComponent<MoveLaser>().StartLaserTimer();
        }
    }
    private void ShootLaser()
    {
        if (isInRange)
            ShootPooledObject();
    }
    private void RotateShip()
    {
        Vector3 lookDirection = (playerPosition.position - transform.position).normalized;
        Quaternion rotateShip = Quaternion.LookRotation(lookDirection);

        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotateShip, rotateSpeed));
    }
    private void MoveShip()
    {
        SetDirection();
        rb.AddRelativeForce(moveDirection * speed);
    }
    private void SetDirection()
    {
        if (!isInRange)
        {
            moveDirection = Vector3.forward;
        }
        else if (isTooClose)
        {
            moveDirection = Vector3.back;
        }
        else
        {
            moveDirection = Vector3.left;
        }
    }

    private void CheckHealth()
    {
        if (healthSystem.GetHealth() <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser"))
        {
            MoveLaser moveLaser = other.GetComponent<MoveLaser>();

            healthSystem.Damage(moveLaser.LaserDamage);

            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.SetActive(false);

            CheckHealth();
        }
        else if (other.CompareTag("Missile"))
        {
            HomingMissile homingMissile = other.GetComponent<HomingMissile>();
            healthSystem.Damage(homingMissile.MissileDamage);
            Destroy(other.gameObject);

            CheckHealth();
        }
    }

}
