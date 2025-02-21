using UnityEngine;
using Photon.Pun;

public class EnemyShooting : MonoBehaviourPun
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireRate = 1.5f;
    private float nextFireTime = 0f;

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return; // ✅ Only Master Client spawns bullets

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null) return;

        GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, firePoint.position, firePoint.rotation);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetShooterTag("Enemy"); // ✅ Assign shooter tag
        }
    }
}
