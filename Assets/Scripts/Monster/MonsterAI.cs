using UnityEngine;

public class MonsterAIMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    private float moveSpeed = 2f; // Speed at which the enemy moves

    [SerializeField]
    private float turnSpeed = 200f; // Speed of rotation

    private Rigidbody2D rb;
    private PlayerDetection playerDetection; // Reference to the player detection system
    private Vector2 currentDirection;
    private float directionChangeTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDetection = GetComponent<PlayerDetection>();
        currentDirection = transform.up; // Initial movement direction
        ResetDirectionChangeTimer();
    }

    private void FixedUpdate()
    {
        CalculateDirection();
        RotateToFaceDirection();
        MoveForward();
    }

    private void CalculateDirection()
    {
        if (playerDetection != null && playerDetection.IsPlayerVisible)
        {
            
            currentDirection = playerDetection.PlayerDirection;
        }
        else
        {
            // Wander randomly if the player isn't detected
            HandleRandomWandering();
        }
    }

    private void HandleRandomWandering()
    {
        directionChangeTimer -= Time.deltaTime; //countown timer from last frame

        if (directionChangeTimer <= 0)
        {
            float randomAngle = Random.Range(-90f, 90f); 
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomAngle); //use quaternion function for rotation
            currentDirection = randomRotation * currentDirection;

            ResetDirectionChangeTimer();
        }
    }

    private void RotateToFaceDirection()
    {
        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg - 90f; //angle towards player
        float smoothAngle = Mathf.MoveTowardsAngle(rb.rotation, angle, turnSpeed * Time.deltaTime); //rotate
        rb.rotation = smoothAngle;
    }

    private void MoveForward()
    {
        rb.linearVelocity = transform.up * moveSpeed;
    }

    private void ResetDirectionChangeTimer()
    {
        directionChangeTimer = Random.Range(1f, 4f); // Set a random interval for direction changes
    }

    public void MoveToward(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    public void Patrol()
    {
      rb.linearVelocity = Vector2.right * moveSpeed * Mathf.Sin(Time.time);  //side patrol
    }

}
