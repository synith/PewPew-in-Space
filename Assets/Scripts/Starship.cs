using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starship : MonoBehaviour
{    
    private float speed = 15;
    private int hullPoints;
    private int shieldPoints;
    private MousePosition2D mousePosition2D;
    private Vector3 moveDirection;

    private void Awake()
    {
        mousePosition2D = GetComponent<MousePosition2D>();
    }
    private void Update()
    {        
        MoveShip();
        RotateShip();
    }

    private void MoveShip()
    {
        transform.Translate(speed * Time.deltaTime * PlayerInput());
    }
    private void RotateShip()
    {
        transform.LookAt(mousePosition2D.MouseWorldPosition);
    }
    private Vector3 PlayerInput()
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveDirection.Normalize();
        return moveDirection;
    }
}
