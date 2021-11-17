using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLaser : MonoBehaviour
{
    public int LaserDamage { get { return _laserDamage; } private set { _laserDamage = value; } } // ENCAPSULATION

    [SerializeField] private int _laserDamage = 10;   
    [SerializeField] private float speed = 180;
    [SerializeField] private float laserLifeTime = 2f;
    [SerializeField] private AudioClip laserClip;
    private AudioSource laserAudioSource;
    
    private Rigidbody rb;

    private void Awake() // starts moving laser forward as soon as script is loaded
    {
        rb = GetComponent<Rigidbody>();
        laserAudioSource = GetComponent<AudioSource>();
        Move(Vector3.forward); // ABSTRACTION
        laserAudioSource.volume = SoundManager.Instance.sfxVolume;
    }
    public void Move(Vector3 direction) // sets rigidbody's velocity in passed direction
    {        
        rb.AddRelativeForce(direction * speed, ForceMode.VelocityChange);
    }

    public void StartLaserTimer() // starts a timer to deactivate and return laser to laser pool
    {        
        StartCoroutine(nameof(LaserLifeTimer));
        Move(Vector3.forward);
        laserAudioSource.PlayOneShot(laserClip, 0.1f);
    }

    IEnumerator LaserLifeTimer() // after set amount of time laser returns to pool
    {
        yield return new WaitForSeconds(laserLifeTime);
        gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
    }
}
