using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform firePoint; // Where the projectile spawns
    public GameObject projectilePrefab; // Projectile to shoot
    public float fireRate = 1.5f; // Time between shots
    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetShooter(gameObject); // Set shooter as the enemy
        }
    }
}
