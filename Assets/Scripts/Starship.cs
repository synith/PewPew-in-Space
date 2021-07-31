using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starship : MonoBehaviour
{    
    [SerializeField] private float speed; // set speed in inspector
    private Vector3 moveDirection;

    // private int hullPoints;
    // private int shieldPoints;    
    private void Update()
    {        
        // ABSTRACTION
        MoveShip();
        RotateShip();
    }

    private void MoveShip()
    {
        transform.Translate(speed * Time.deltaTime * PlayerInput());
    }
    private void RotateShip()
    {
        transform.LookAt(MousePosition2D.MouseWorldPosition);
    }
    private Vector3 PlayerInput()
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveDirection.Normalize();
        return moveDirection;
    }
}
