using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLaser : MonoBehaviour
{
    public int LaserDamage { get; private set; }
   
    [SerializeField] private float speed;
    [SerializeField] private float laserLifeTime;
    
    private Rigidbody rb;

    private void Awake()
    {
        int _laserDamage = 10;
        LaserDamage = _laserDamage;
        rb = GetComponent<Rigidbody>();
        Move(Vector3.forward);
    }
    public void Move(Vector3 direction)
    {        
        rb.AddRelativeForce(direction * speed, ForceMode.VelocityChange);
    }

    public void StartLaserTimer()
    {        
        StartCoroutine(nameof(LaserLifeTimer));
        Move(Vector3.forward);
    }

    IEnumerator LaserLifeTimer()
    {
        yield return new WaitForSeconds(laserLifeTime);
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
    }
}
