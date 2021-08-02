using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform shootPoint;
    private Transform playerPosition;

    private Vector3 moveDirection;
    private Rigidbody rb;

    private bool inRange = false;
    private float minRange = 60;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        FindPlayer();        
        InvokeRepeating(nameof(CheckDistance), 0.1f, 0.1f);
    }
    private void Update()
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
        // Debug.Log("Enemy reads player at distance - " + dist);

        if (dist < minRange)
            inRange = true;
        else
            inRange = false;
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
        if (!inRange)
            moveDirection = Vector3.forward;
        else
            moveDirection = Vector3.back;
    }
    
}
