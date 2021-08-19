using UnityEngine;
public class EnemyController : Starship
{
    [SerializeField] private float checkDistanceSeconds;
    private Transform playerPosition;
    private bool isTooClose;
    private readonly float minRange = 80;
    private readonly float closeRange = 40;
    protected override void Awake()
    {
        base.Awake();
        playerPosition = FindObjectOfType<PlayerController>().transform;
    }    
    protected override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(CheckDistance), checkDistanceSeconds, checkDistanceSeconds);
        InvokeRepeating(nameof(ShootLaser), 0.5f, 0.5f);
    }
    protected override void MoveShip()
    {
        SetDirection();
        base.MoveShip();
    }
    protected override void CheckDeath()
    {
        if (healthSystem.GetHealth() <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.AddPoint(5);
        }
    }
    protected override Vector3 GetLookDirection()
    {
        Vector3 lookDirection = (playerPosition.position - transform.position).normalized;
        return lookDirection;
    }   
    private void CheckDistance()
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
    private void ShootLaser()
    {
        if (isInRange && !GameManager.Instance.GameOver)
            ShootPooledLaser();
    }    
    private void SetDirection()
    {
        if (!isInRange) moveDirection = Vector3.forward;        
        else if (isTooClose) moveDirection = Vector3.back;        
        else moveDirection = Vector3.left;       
    }
}
