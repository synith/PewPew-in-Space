using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{    
    [SerializeField] private float speed; // set speed in inspector
    [SerializeField] private float rotateSpeed;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private int maxHealth;
 

    private HealthSystem healthSystem;

    private Vector3 moveDirection;

    private Rigidbody rb;

    private bool isInRange;
    private float minRange = 80f;
    

    // private int hullPoints;
    // private int shieldPoints;

    private void Start()
    {
        HealthBar healthBar = GetComponentInChildren<HealthBar>();

        rb = GetComponent<Rigidbody>();
        healthSystem = new HealthSystem(maxHealth);
        
        healthBar.SetUp(healthSystem);
    }
    private void Update()
    {        
        // ABSTRACTION
        MoveShip();
        RotateShip();
    }

    private void MoveShip()
    {
        rb.AddRelativeForce(moveDirection * speed);
    }

    private void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        moveDirection = new Vector3(inputVec.x, 0, inputVec.y);
    }

    private void OnShootLaser()
    {
        GameObject laser = LaserPool.SharedInstance.GetPooledObject();
        if (laser != null)
        {
            laser.transform.SetPositionAndRotation(shootPoint.position, transform.rotation);
            laser.SetActive(true);
            laser.GetComponent<MoveLaser>().StartLaserTimer();
        }

    }
    private void OnShootMissile()
    {
        Transform target = GetClosestEnemy();
        CheckDistance(target);

        if (target != null && isInRange)
        {
            GameObject tempMissile;
            tempMissile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            tempMissile.GetComponent<HomingMissile>().Fire(target);
        }
        else
        {
            Debug.Log("No Target in Range");
        }
        
    }
    private void CheckDistance(Transform target)
    {
        float dist = Vector3.Distance(target.position, transform.position);

        if (dist < minRange)
            isInRange = true;
        else
            isInRange = false;
    }
    private void OnShield()
    {
        Debug.Log("Shield Activated!");
    }
    private void RotateShip()
    {
        Vector3 lookDirection = (MousePosition2D.MouseWorldPosition - transform.position).normalized;
        Quaternion rotateShip = Quaternion.LookRotation(lookDirection);

        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotateShip, rotateSpeed));
    }
    
    private Transform GetClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        return bestTarget;
    }

    private void CheckHealth()
    {
        if (healthSystem.GetHealth() <= 0)
        {
            gameObject.SetActive(false);
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
    }
}
