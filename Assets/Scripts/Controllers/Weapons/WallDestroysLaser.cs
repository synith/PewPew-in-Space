using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestroysLaser : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) // if laser hits wals return laser to pooled lasers
    {
        if (other.CompareTag("Laser"))
        {
            ReturnToPool(other);
        }
    }
    protected void ReturnToPool(Collider other) // when returning laser to pool, sets the velocity to zero and the gameobject false
    {
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        other.gameObject.SetActive(false);
    }
}
