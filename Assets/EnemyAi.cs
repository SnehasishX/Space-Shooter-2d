using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 2f; // Speed of enemy movement
    public float detectionRange = 10f; // Range at which enemy detects the player

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check if player is within detection range
        if (distanceToPlayer <= detectionRange)
        {
            FollowPlayer();
            RotateTowardsPlayer();
        }
    }

    void FollowPlayer()
    {
        // Move towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void RotateTowardsPlayer()
    {
        // Get direction to player
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Rotate enemy to face the player
        transform.rotation = Quaternion.Euler(0, 0, angle+180);
    }
}
