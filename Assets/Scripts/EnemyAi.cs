using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Player reference
    // public GameObject bulletPrefab;
    // public Transform firePoint;
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public float fireRate = 1.5f;
    private float nextFireTime;

    void Start()
    {
        FindPlayer(); // Ensure we have the player reference
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer(); // Try finding the player again if null
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            FollowPlayer();
            RotateTowardsPlayer();

            if (Time.time >= nextFireTime)
            {
                // ShootPlayer();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle+180);
    }

    // void ShootPlayer()
    // {
    //     Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    // }

    public void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    
}
