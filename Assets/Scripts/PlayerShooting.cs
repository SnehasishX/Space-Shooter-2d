using UnityEngine;
using Photon.Pun;

public class PlayerShooting : MonoBehaviourPun
{
    public Transform firePoint;
    public GameObject[] projectilePrefabs;
    public int selectedProjectileIndex = 0;

    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    void Update()
    {
        if (!photonView.IsMine) return; // Only the local player can shoot

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot(); // Only the local player instantiates the bullet
            nextFireTime = Time.time + fireRate;
        }

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
            // Only the shooter instantiates the bullet, and it is automatically synced
            GameObject projectile = PhotonNetwork.Instantiate(projectilePrefabs[selectedProjectileIndex].name, firePoint.position, firePoint.rotation);
            
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetShooter(gameObject);
            }
        }
    }
}
