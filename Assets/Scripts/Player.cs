using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{    
    [SerializeField] private float speed; // set speed in inspector
    private Vector3 moveDirection;

    private Rigidbody rb;

    private PlayerInput controls;

    // private int hullPoints;
    // private int shieldPoints;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        controls = GetComponent<PlayerInput>();
    }
    private void Update()
    {        
        // ABSTRACTION
        MoveShip();
        RotateShip();
    }

    private void MoveShip()
    {
        rb.AddForce(moveDirection * speed);
    }

    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        moveDirection = new Vector3(inputVec.x, 0, inputVec.y);
    }

    public void OnShield()
    {
        Debug.Log("Shield Activated!");
    }
    private void RotateShip()
    {
       transform.LookAt(MousePosition2D.MouseWorldPosition);
    }     
}
