using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMissile : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    private void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
