using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CamFollow))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpawnDoorTrigger))]
public class PlayerController : Starship // INHERITANCE
{
    [SerializeField] private float missileRange = 100f;
    [SerializeField] private float missileCoolDown = 2f;
    [SerializeField] private int missileCount = 3;
    [SerializeField] private float laserCoolDownSeconds = 0.2f;
    [SerializeField] private int healthPickupHealAmount = 20;

    [SerializeField] private AudioClip noAmmoSound;
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip pickupSound;


    private bool isMissileReady = true;
    private bool isMissileArmed = false;
    private bool isLaserReady = true;

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.SetMissileCountText(missileCount);
    }
    public HealthSystem GetHealthSystem() => healthSystem;
    private void OnPause() // if pause key is pressed sets game state to paused
    {
        GameManager.Instance.GamePaused = true;
    }
    private void OnMove(InputValue input) // sets move direction based on Move keys being pressed
    {
        Vector2 inputVec = input.Get<Vector2>();
        moveDirection = new Vector3(inputVec.x, 0, inputVec.y);
    }
    protected override void MoveShip()
    {
        rb.AddForce(moveDirection * speed);
    }
    private void OnShield(InputValue input) // checks to see if shield button is held down and sets shield bool and player tag accordingly
    {
        if (input.Get<float>() > 0.9f)
        {
            isShieldActive = true;
            gameObject.tag = "Shield";
            if (shieldDown)
            {
                StartCoroutine(shield.GetComponent<Shield>().ShieldRegen());
            }            
        }
        else
        {
            isShieldActive = false;
            gameObject.tag = "Player";
        }
    }
    private void OnShootLaser() // shoots a pooled laser when shoot laser key is pressed
    {
        if (!GameManager.Instance.GamePaused && !isShieldActive && isLaserReady)
        {
            ShootPooledLaser();
            isLaserReady = false;
            StartCoroutine(nameof(LaserCoolDown));
        }
    }
    private IEnumerator LaserCoolDown()
    {
        yield return new WaitForSeconds(laserCoolDownSeconds);
        isLaserReady = true;
    }
    private void OnShootMissile() // shoots a missile when shoot missile key is pressed
    {
        if (!GameManager.Instance.GamePaused)
        {
            if (isMissileReady && missileCount > 0) // only shoot missile if missile count is greater than 0
            {
                FireMissileAtTarget();
            }
            else if (!isMissileReady)
            {
                GameManager.Instance.ShowStatus("Missile on Cooldown"); // updates status text informing player missile is still on cooldown
                starshipAudio.PlayOneShot(errorSound, 0.1f);
            }
            else if (missileCount < 1)
            {
                GameManager.Instance.ShowStatus("Out of missiles"); // updates status text informing player there are no more missiles to fire
                starshipAudio.PlayOneShot(noAmmoSound, 0.1f);
            }
        }
    }
    private void FireMissileAtTarget()
    {
        Transform target = GetClosestEnemy();
        CheckRange(target);
        if (target != null && isInRange) // only shoots missile if there is a target, and the target is in range
        {
            FireMissile(target);
        }
        else
        {
            TargetNotInRange();
        }
    }
    private void FireMissile(Transform target)
    {
        missileCount--;
        GameManager.Instance.SetMissileCountText(missileCount);

        isMissileArmed = false;
        StartCoroutine(nameof(MissileArmed));

        SpawnAndFireMissileAtTarget(target);

        isMissileReady = false;
        StartCoroutine(nameof(MissileCoolDown)); // starts missile cooldown preventing missile from being fired again until after cooldown
    }
    private void SpawnAndFireMissileAtTarget(Transform target)
    {
        GameObject tempMissile = Instantiate(missilePrefab, shootPoint.position, transform.rotation);
        tempMissile.GetComponent<HomingMissile>().Fire(target);
    }
    private void TargetNotInRange()
    {
        GameManager.Instance.ShowStatus("No Target in Range"); // updates status text informing player there is no target in range
                                                               // play sad sound
        starshipAudio.PlayOneShot(errorSound, 0.1f);
    }
    private Transform GetClosestEnemy() // creates an array of objects that have the enemy tag and compares their distance to see which is closest, returns this enemy as the closest target
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }
        return bestTarget;
    }
    private void CheckRange(Transform target) // checks to see if target is in range of missiles
    {
        if (target != null)
        {
            float dist = Vector3.Distance(target.position, transform.position);
            if (dist < missileRange) isInRange = true;
            else isInRange = false;
        }
    }
    private IEnumerator MissileCoolDown() // cooldown to prevent missiles from being spammed
    {
        yield return new WaitForSeconds(missileCoolDown);
        isMissileReady = true;
    }
    private IEnumerator MissileArmed()
    {
        yield return new WaitForSeconds(0.5f);
        isMissileArmed = true;
    }
    protected override Vector3 GetLookDirection() // gets mouse position relative to player and sets this as the look direction
    {
        Vector3 lookDirection = (MousePosition2D.MouseWorldPosition - transform.position).normalized;
        return lookDirection;
    }
    protected override void CheckDeath() // if player's health reaches 0 set the game state to GameOver and disable the player object
    {
        if (healthSystem.GetHealth() <= 0)
        {
            PlayerDies();
        }
    }
    private void PlayerDies()
    {
        SoundManager.Instance.PlaySound(deathSound);
        GameObject deathParticleInstance = Instantiate(deathParticle, transform.position, transform.rotation);
        Destroy(deathParticleInstance, 2f);
        gameObject.SetActive(false);
        GameManager.Instance.GameOver = true;
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser") && gameObject.CompareTag("Player")) // if player hit by laser with no shield, then do laser damage
        {
            starshipAudio.PlayOneShot(hullHitSound, 0.1f);
            PlayWeaponParticle(other, laserHullParticle);
            LaserHit(other);
        }
        else if (other.CompareTag("Missile") && gameObject.CompareTag("Player") && isMissileArmed)
        {
            starshipAudio.PlayOneShot(missileHitSound, 0.1f);
            PlayWeaponParticle(other, missileHullParticle);
            MissileHit(other);
        }
        else if (other.CompareTag("Laser") && gameObject.CompareTag("Shield")) // if shield up, simply return the laser to the pool
        {
            PlayWeaponParticle(other, laserShieldParticle);
            ReturnToPool(other);
        }
        else if (other.CompareTag("Pickup_Health"))
        {
            Destroy(other.gameObject);
            starshipAudio.PlayOneShot(pickupSound, 0.1f);
            healthSystem.Heal(healthPickupHealAmount);
        }
        else if (other.CompareTag("Pickup_Missile"))
        {
            Destroy(other.gameObject);
            starshipAudio.PlayOneShot(pickupSound, 0.1f);
            if (missileCount < 3)
            {
                missileCount++;
                GameManager.Instance.SetMissileCountText(missileCount);
            }
        }
    }  
}
