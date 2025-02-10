using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public bool IsPlayerVisible { get; private set; } 
    public Vector2 PlayerDirection { get; private set; } // move towards player

    [SerializeField] private float detectionRadius = 5f; // radius
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // Uses player tag to find player
    }

    private void Update()
    {
        if (player != null)
        {
            Vector2 directionToPlayer = player.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude; //length of vector
            //player is in range
            if (distanceToPlayer <= detectionRadius)
            {
                IsPlayerVisible = true;
                PlayerDirection = directionToPlayer.normalized;
            }
            else
            {
                IsPlayerVisible = false;
            }
        }
    }
}
