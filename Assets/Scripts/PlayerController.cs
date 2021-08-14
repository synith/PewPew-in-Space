using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : Starship
{
    [SerializeField] private readonly float missileRange = 100f; 

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
    }    
}
