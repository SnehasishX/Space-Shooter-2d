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
        if (!PhotonNetwork.IsMasterClient) return; // 🔥 Only the Master Client shoots

        if (Time.time >= nextFireTime)
        {
            photonView.RPC("Shoot", RpcTarget.All);
            nextFireTime = Time.time + fireRate;
        }
    }

    [PunRPC]
    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript?.SetShooter(gameObject);
    }
}
