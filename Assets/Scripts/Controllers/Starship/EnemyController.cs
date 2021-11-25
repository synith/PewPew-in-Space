using UnityEngine;
public class EnemyController : Starship // INHERITANCE
{
    [SerializeField] private float checkDistanceSeconds;
    private Transform playerPosition;
    private bool isTooClose;
    private bool goRight;
    private bool hasHitWall;
    private bool isDead;
    private readonly float minRange = 120;
    private readonly float closeRange = 40;

    [SerializeField] private GameObject healthPickupPrefab;
    [SerializeField] private GameObject missilePickupPrefab;
    [SerializeField] private AudioClip dropPickupSound;

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
        PickStartDirection();
    }
    private void PickStartDirection()
    {
        int dice;
        dice = Random.Range(0, 2);
        if (dice > 0)
            goRight = true;
        else
            goRight = false;
    }
    protected override void MoveShip()
    {
        SetDirection();  // ABSTRACTION
        base.MoveShip(); // POLYMORPHISM
    }
    protected override void CheckDeath() // when enemy dies, destroy object and add points to score
    {
        if (healthSystem.GetHealth() <= 0 && !isDead)
        {
            isDead = true;
            CheckIfDroppingPickup(transform);
            EnemyDies();            
            GameManager.Instance.CountEnemiesDeath();

            if (GameManager.Instance.EnemiesDefeatedCount >= GameManager.Instance.TotalEnemies)
            {
                if (!GameManager.Instance.EndlessMode)
                {
                    GameManager.Instance.GameWon = true;
                }
            }
            if (GameManager.Instance.EnemiesDefeatedInRoom >= GameManager.Instance.TotalEnemiesInRoom)
            {
                GameManager.Instance.SpawnManager.OpenDoor(GameManager.Instance.SpawnManager.CurrentRoomNumber);
                GameManager.Instance.EnemiesDefeatedInRoom = 0;
            }
        }
    }
    private void EnemyDies()
    {
        GameObject deathParticleInstance = Instantiate(deathParticle, transform.position, transform.rotation);
        Destroy(deathParticleInstance, 2f);
        Destroy(gameObject);
        SoundManager.Instance.PlaySound(deathSound);        
        GameManager.Instance.AddPoint(5);
    }
    private void CheckIfDroppingPickup(Transform transform)
    {
        int rollDice;
        rollDice = Random.Range(0, 100);
        if (rollDice < 10)
        {
            Debug.Log("Dropped HP!");
            starshipAudio.PlayOneShot(dropPickupSound, 0.1f);
            SoundManager.Instance.PlaySound(dropPickupSound);
            Instantiate(healthPickupPrefab, transform.position, missilePickupPrefab.transform.rotation);
        }
        else if (rollDice < 20)
        {
            Debug.Log("Dropped missile!");
            starshipAudio.PlayOneShot(dropPickupSound, 0.1f);
            SoundManager.Instance.PlaySound(dropPickupSound);
            Instantiate(missilePickupPrefab, transform.position, missilePickupPrefab.transform.rotation);
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
