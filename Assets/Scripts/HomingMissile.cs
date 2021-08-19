using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public int MissileDamage { get { return _missileDamage; } private set { _missileDamage = value; } }

    [Header("Rocket Settings")]
    [SerializeField] private int _missileDamage = 40;
    [SerializeField] private float flySpeed = 110f;
    [SerializeField] private float turnSpeed = 20f;
    [SerializeField] private float missileLifeTimer = 3f;

    private Transform targetTransform;
    private Rigidbody missileRigidbody;

    private bool missileFired;

    private Transform missileTransform;

    void Start()
    {
        if (!targetTransform)  // if no rocket target then ask for a target
            Debug.Log("Please set a target.");

        missileRigidbody = GetComponent<Rigidbody>();
        missileTransform = GetComponent<Transform>();
    }
    public void Fire(Transform newTarget) // fires missile at target, destroying missile after set amount of time
    {
        targetTransform = newTarget;
        missileFired = true;
        Destroy(gameObject, missileLifeTimer);
    }
    private void FixedUpdate()
    {
        if (!missileRigidbody) // if no missile rigid body do nothing
            return;
        if (missileFired && targetTransform != null) // if missile has been fired and there is a target, then rotate and move towards target
        {
            Quaternion missileTargetRotation = Quaternion.LookRotation(targetTransform.position - missileTransform.position).normalized; // new quaternion made of a vector3 that takes the target position minus the missile position
            missileRigidbody.MoveRotation(Quaternion.RotateTowards(missileTransform.rotation, missileTargetRotation, turnSpeed)); // rotate object from current rotation towards target rotation at turn speed
            missileRigidbody.velocity = (missileTransform.forward * flySpeed); // add force forward
        }
    }
}
