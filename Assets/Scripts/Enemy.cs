using UnityEngine;
using Photon.Pun;

public class Enemy : MonoBehaviourPun
{
    public int maxHealth = 50;
    private int health;

    public GameObject floatingDamagePrefab;

    void Start()
    {
        health = maxHealth;
    }

    [PunRPC] // Ensures damage syncs across the network
    public void TakeDamage(int damage)
    {
        ShowFloatingDamage(damage);

        health -= damage;

        if (health <= 0)
        {
            if (PhotonNetwork.IsMasterClient) // Only Master Client destroys the enemy
            {
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.UpdateScore(10);
                }

                EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
                if (spawner != null)
                {
                    spawner.RemoveEnemy(gameObject);
                }

                PhotonNetwork.Destroy(gameObject); // Destroy enemy for all players
            }
        }
    }

    void ShowFloatingDamage(int damage)
    {
        if (floatingDamagePrefab == null) return;

        GameObject damageText = Instantiate(floatingDamagePrefab, transform.position, Quaternion.identity);
        FloatingDamage floatingDamage = damageText.GetComponent<FloatingDamage>();
        if (floatingDamage != null)
        {
            floatingDamage.SetDamageText(damage);
        }
    }
}
