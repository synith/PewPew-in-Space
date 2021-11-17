using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    [SerializeField] GameObject doorReplacementWall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hull") || other.CompareTag("Shield"))
        {
            doorReplacementWall.SetActive(true);
        }
    }
}
