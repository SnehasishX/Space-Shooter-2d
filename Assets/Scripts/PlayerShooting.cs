using UnityEngine;
using Photon.Pun;

public class PlayerShooting : MonoBehaviourPun
{
    public Transform firePoint;
    public GameObject[] projectilePrefabs;
    public int selectedProjectileIndex = 0;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    public GameObject FloatingDmg; // ✅ Ensure this is assigned in the Inspector

    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
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
            GameObject projectile = PhotonNetwork.Instantiate(
                projectilePrefabs[selectedProjectileIndex].name, 
                firePoint.position, 
                firePoint.rotation
            );

            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetShooterTag("Player"); // ✅ Set shooter as Player
            }
        }
    }

    // ✅ Ensure this method exists and is correctly marked as an RPC
    [PunRPC]
    public void ShowDamageText(Vector3 position, int damage)
    {
        if (FloatingDmg == null) return;

        GameObject damageText = Instantiate(FloatingDmg, position, Quaternion.identity);
        FloatingDamage floatingDamage = damageText.GetComponent<FloatingDamage>();

        if (floatingDamage != null)
        {
            floatingDamage.SetDamageText(damage);
        }
    }
}
