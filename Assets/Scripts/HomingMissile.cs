using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float rotationSpeed = 200f;
    public float trackingDistance = 15f;

    public Transform target;
    private bool isTracking = true;
    private float lastDistance;

    void Start()
    {
        FindClosestTarget();
    }

    void Update()
    {
        if (target == null)
        {
            MoveStraight();
        }
        else
        {
            float currentDistance = Vector2.Distance(transform.position, target.position);

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
    }

    private void MoveStraight()
    {
        transform.position += transform.right * Time.deltaTime; // Speed should be assigned externally
    }

    private void TrackTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.position += transform.right * Time.deltaTime; // Speed should be assigned externally
    }

    private void FindClosestTarget()
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
}
