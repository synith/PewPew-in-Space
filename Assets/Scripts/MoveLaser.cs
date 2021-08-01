using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLaser : MonoBehaviour
{
    private float speed = 75f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move(Vector3.forward);
    }

    private void Move(Vector3 direction)
    {
        rb.velocity = direction * speed;
    }
}
