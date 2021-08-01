using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This script grabs the mouse position from the camera and converts it to "world space" (where stuff in the unity editor happens)
// Camera needs to be in orthonagal mode (2d mode)
public class MousePosition2D : MonoBehaviour
{

    // ENCAPSULATION
    public static Vector3 MouseWorldPosition { get; private set; }
    private Vector2 mousePosition; 

    [SerializeField] private PlayerInput controls; // use inspector to reference player's PlayerInput component

    [SerializeField] private Camera mainCamera; // use inspector to reference Main Camera
    
    private void Update()
    {
        GetMousePosition();
    }
    private Vector3 GetMousePosition()
    {
        mousePosition = controls.actions["MousePosition"].ReadValue<Vector2>();
        Vector3 _mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        _mouseWorldPosition.y = 15f;
        MouseWorldPosition = _mouseWorldPosition;
        return MouseWorldPosition;
    }
}
