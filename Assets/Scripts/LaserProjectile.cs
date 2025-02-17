using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public float laserLength = 10f;  // Max distance the laser can reach
    public int damage = 10;          // Damage dealt by the laser
    public LineRenderer lineRenderer;
    public float laserDuration = 0.2f;  // How long the laser stays visible

    void Start()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        // Get the start position (the shooter's position)
        Vector3 startPos = transform.position;
        Vector3 direction = transform.right;  // Shoots in the object's forward direction

        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, laserLength);

        if (hit.collider != null)  // If the laser hits something
        {
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, hit.point);

            // Damage the enemy
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        else  // If the laser doesn't hit anything
        {
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, startPos + direction * laserLength);
        }

        // Destroy the laser after a short delay
        Destroy(gameObject, laserDuration);
    }
}
