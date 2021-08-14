using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestroysLaser : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser"))
        {
            ReturnToPool(other);
        }
    }
    protected void ReturnToPool(Collider other)
    {
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        other.gameObject.SetActive(false);
    }
}
