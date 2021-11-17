using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField]private Camera mainCamera;
    private Vector3 offset;
    private void Awake()
    {
        offset = mainCamera.transform.position - transform.position;
    }
    private void Update()
    {
        mainCamera.transform.position = transform.position + offset;
    }
}
