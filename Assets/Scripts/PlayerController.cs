using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : Starship
{
    [SerializeField] private readonly float missileRange = 100f;


    // Put this in a CheckDoor script??
    private bool door1Open;
    private bool door2Open;
    private bool door3Open;
    private bool door4Open;
    private bool door5Open;
    // all of this ^

    private void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        moveDirection = new Vector3(inputVec.x, 0, inputVec.y);
    }
    private void OnShield(InputValue input)
    {
        if (input.Get<float>() > 0.9f)
        {
            isShieldActive = true;
            gameObject.tag = "Shield";
        }
        else
        {
            isShieldActive = false;
            gameObject.tag = "Player";
        }
    }
    private void OnShootLaser()
    {
        ShootPooledLaser();
    }
    private void OnShootMissile()
    {
        Transform target = GetClosestEnemy();
        CheckRange(target);
        if (target != null && isInRange)
        {
            Debug.Log("Firing at " + target.name);
            GameObject tempMissile;
            tempMissile = Instantiate(missilePrefab, shootPoint.position, Quaternion.identity);
            tempMissile.GetComponent<HomingMissile>().Fire(target);
        }
        else
        {
            Debug.Log("No Target in Range");
            // play sad sound
        }
    }
    private Transform GetClosestEnemy()
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
    private void CheckRange(Transform target)
    {
        if (target != null)
        {
            float dist = Vector3.Distance(target.position, transform.position);
            if (dist < missileRange) isInRange = true;
            else isInRange = false;
        }
    }
    protected override Vector3 GetLookDirection()
    {
        Vector3 lookDirection = (MousePosition2D.MouseWorldPosition - transform.position).normalized;
        return lookDirection;
    }
    protected override void CheckDeath()
    {
        if (healthSystem.GetHealth() <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.gameOver = true;
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Laser") && gameObject.CompareTag("Player"))
        {
            LaserHit(other);
        }
        else if (other.CompareTag("Laser") && gameObject.CompareTag("Shield"))
        {
            ReturnToPool(other);
            // do damage to shield
        }
        else if (other.CompareTag("Door"))
        {
            // check which room you're in
            // spawn the enemies in the room
            CheckDoor(other);
        }
    }
    private void CheckDoor(Collider other) // could this be its own script??
    {
        int room;
        if (other.name == "Door1" && !door1Open)
        {
            room = 1;
            door1Open = true;
            GameManager.Instance.spawnManager.SpawnFighter(room);
        }
        else if (other.name == "Door2" && !door2Open)
        {
            room = 2;
            door2Open = true;
            GameManager.Instance.spawnManager.SpawnFighter(room);
        }
        else if (other.name == "Door3" && !door3Open)
        {
            room = 3;
            door3Open = true;
            GameManager.Instance.spawnManager.SpawnFighter(room);
        }
        else if (other.name == "Door4" && !door4Open)
        {
            room = 4;
            door4Open = true;
            GameManager.Instance.spawnManager.SpawnFighter(room);
        }
        else if (other.name == "Door5" && !door5Open)
        {
            room = 5;
            door5Open = true;
            GameManager.Instance.spawnManager.SpawnFighter(room);
        }
        
    }
}
