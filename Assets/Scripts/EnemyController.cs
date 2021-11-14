using UnityEngine;
public class EnemyController : Starship // INHERITANCE
{
    [SerializeField] private float checkDistanceSeconds;
    private Transform playerPosition;
    private bool isTooClose;
    private bool goRight;
    private bool hasHitWall;
    private readonly float minRange = 120;
    private readonly float closeRange = 40;
    protected override void Awake() // find player's position on script loading
    {
        base.Awake(); // POLYMORPHISM
        playerPosition = FindObjectOfType<PlayerController>().transform;
    }    
    protected override void Start() // start repeatedly checking distance to player and shooting at player
    {
        base.Start(); // POLYMORPHISM
        InvokeRepeating(nameof(CheckDistance), checkDistanceSeconds, checkDistanceSeconds);
        InvokeRepeating(nameof(ShootLaser), 0.5f, 0.5f);
    }
    protected override void MoveShip()
    {
        SetDirection();  // ABSTRACTION
        base.MoveShip(); // POLYMORPHISM
    }
    protected override void CheckDeath() // when enemy dies, destroy object and add points to score
    {
        if (healthSystem.GetHealth() <= 0)
        {
            SoundManager.Instance.PlaySound(deathSound);
            Destroy(gameObject);
            GameManager.Instance.AddPoint(5);
            GameManager.Instance.EnemiesDefeatedCount++;

            if (GameManager.Instance.EnemiesDefeatedCount >= GameManager.Instance.TotalEnemies)
            {
                GameManager.Instance.GameWon = true;
            }
        }
    }
    protected override Vector3 GetLookDirection() // sets look direction towards player's position
    {
        Vector3 lookDirection = (playerPosition.position - transform.position).normalized;
        return lookDirection;
    }   
    private void CheckDistance() // checks distance from player to determine range states
    {
        float dist = Vector3.Distance(playerPosition.position, transform.position);

        if (dist < minRange)
            isInRange = true;
        else
            isInRange = false;
        if (dist < closeRange) 
            isTooClose = true;
        else
            isTooClose = false;
    }
    private void ShootLaser() // shoots laser while in range of player and game not over
    {
        if (isInRange && !GameManager.Instance.GameOver)
            ShootPooledLaser();
    }    
    private void SetDirection() // sets enemy direction based on distance to player
    {
        if (!isInRange) moveDirection = Vector3.forward;     // if not in range move forward
        else if (isTooClose) moveDirection = Vector3.back;   // if too close then move back     
        else moveDirection = GetStrafeDirection();           // if in range and not too close then strafe
    }
    private Vector3 GetStrafeDirection()
    {
        Vector3 strafeDirection;
        if (goRight) strafeDirection = Vector3.right;
        else strafeDirection = Vector3.left;
        return strafeDirection;
    }
    private void ChangeStrafeDirection()
    {
        goRight = !goRight;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ChangeStrafeDirection();
        }
    }
}
