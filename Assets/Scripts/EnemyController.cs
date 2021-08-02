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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        FindPlayer();        
        InvokeRepeating(nameof(CheckDistance), 1f, 1f);
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
        Debug.Log("Enemy reads player at distance - " + dist);
    }    
    private void RotateShip()
    {
        Vector3 lookDirection = (playerPosition.position - transform.position).normalized;
        Quaternion rotateShip = Quaternion.LookRotation(lookDirection);

        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotateShip, rotateSpeed));
    }
    private void MoveShip()
    {
        moveDirection = Vector3.forward;
        rb.AddRelativeForce(moveDirection * speed);
    }
    
}
