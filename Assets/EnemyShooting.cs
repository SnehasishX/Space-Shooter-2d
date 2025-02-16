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
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
