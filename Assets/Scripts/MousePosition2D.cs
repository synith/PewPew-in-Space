using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script grabs the mouse position from the camera and converts it to "world space" (where stuff in the unity editor happens)
// Camera needs to be in orthonagal mode (2d mode)
public class MousePosition2D : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public Vector3 MouseWorldPosition { get; private set; }

    private void Update()
    {
        GetMousePosition();
    }

    private Vector3 GetMousePosition()
    {
        Vector3 _mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _mouseWorldPosition.y = 15f;
        MouseWorldPosition = _mouseWorldPosition;
        return MouseWorldPosition;
    }
}
