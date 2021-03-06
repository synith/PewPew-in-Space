using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This script grabs the mouse position from the camera and converts it to "world space" (where stuff in the unity editor happens)
// Camera needs to be in orthonagal mode (2d mode)
public class MousePosition2D : MonoBehaviour
{    
    public static Vector3 MouseWorldPosition { get; private set; } // ENCAPSULATION
    private Vector2 mousePosition; 

    [SerializeField] private PlayerInput controls; // use inspector to reference player's PlayerInput component

    [SerializeField] private Camera mainCamera; // use inspector to reference Main Camera
    
    private void Update()
    {
        GetMousePosition();
    }
    private Vector3 GetMousePosition() // takes mouse position and converts to x and z coordinates in world space
    {
        mousePosition = controls.actions["MousePosition"].ReadValue<Vector2>();
        Vector3 _mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        _mouseWorldPosition.y = 0f;
        MouseWorldPosition = _mouseWorldPosition;
        return MouseWorldPosition;
    }
}
