using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public int MissileDamage { get; private set; }

    private Transform targetTransform;
    private Rigidbody missileRigidbody;

    [Header("Rocket Settings")]
    [SerializeField] private float flySpeed = 10f;
    [SerializeField] private float turnSpeed = 1f;
    [SerializeField] private float missileLifeTimer = 5f;

    private bool missileFired;

    private Transform missileTransform;

    void Start()
    {
        int _missileDamage = 40;
        MissileDamage = _missileDamage;

        if (!targetTransform)  // if no rocket target then ask for a target
            Debug.Log("Please set a target.");

        missileRigidbody = GetComponent<Rigidbody>();
        missileTransform = GetComponent<Transform>();
    }
    public void Fire(Transform newTarget)
    {
        targetTransform = newTarget;
        missileFired = true;
        Destroy(gameObject, missileLifeTimer);
    }
    private void FixedUpdate()
    {
        if (!missileRigidbody) // if no missile rigid body do nothing
            return;
        if (missileFired && targetTransform != null)
        {
            Quaternion missileTargetRotation = Quaternion.LookRotation(targetTransform.position - missileTransform.position).normalized; // new quaternion made of a vector3 that takes the target position minus the missile position
            missileRigidbody.MoveRotation(Quaternion.RotateTowards(missileTransform.rotation, missileTargetRotation, turnSpeed)); // rotate player from current rotation towards enemy at turn speed
            missileRigidbody.velocity = (missileTransform.forward * flySpeed); // add force forward
        }
    }
}
