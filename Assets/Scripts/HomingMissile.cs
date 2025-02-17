using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public Transform target;           // Target to track
    public float speed = 10f;          // Speed of the missile
    public float rotationSpeed = 200f; // How fast it turns
    public float maxLifetime = 5f;     // Time before self-destruction
    public float trackingDistance = 15f; // Max tracking range

    private bool isTracking = true;    // Whether the missile is tracking
    private float startTime;           // Time when missile was spawned
    private float lastDistance;        // Store last frame's distance

    void Start()
    {
        startTime = Time.time;
        FindClosestTarget();
    }

    void Update()
    {
        if (target == null)
        {
            MoveStraight(); // No target found, move straight
        }
        else
        {
            float currentDistance = Vector2.Distance(transform.position, target.position);

            // Switch tracking off if missile is moving away from the target
            if (currentDistance > lastDistance)
            {
                isTracking = false;
            }

            lastDistance = currentDistance;

            if (isTracking)
            {
                TrackTarget();
            }
            else
            {
                MoveStraight();
            }
        }

        // Destroy after max lifetime
        if (Time.time - startTime > maxLifetime)
        {
            Destroy(gameObject);
        }
    }

    void TrackTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.position += transform.right * speed * Time.deltaTime;
    }

    void MoveStraight()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    void FindClosestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = trackingDistance;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy.transform;
            lastDistance = Vector2.Distance(transform.position, target.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Destroy enemy
            Destroy(gameObject); // Destroy missile
        }
    }
}
