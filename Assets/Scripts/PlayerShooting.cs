using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint; // The point from where the projectile is fired
    public GameObject[] projectilePrefabs; // Array of different projectiles
    public int selectedProjectileIndex = 0; // The current projectile type

    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        // Switch projectile type with number keys (1,2,3,4...)
        for (int i = 0; i < projectilePrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedProjectileIndex = i;
            }
        }
    }

    void Shoot()
    {
        if (projectilePrefabs.Length > 0 && selectedProjectileIndex < projectilePrefabs.Length)
        {
            GameObject projectile = Instantiate(projectilePrefabs[selectedProjectileIndex], firePoint.position, firePoint.rotation);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetShooter(gameObject); // Set shooter as the player
            }
        }
    }
}
