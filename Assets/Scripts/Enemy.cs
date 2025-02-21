using UnityEngine;
using Photon.Pun;

public class Enemy : MonoBehaviourPun
{
    public int maxHealth = 50;
    private int health;
    public int scoreValue = 10; // ✅ Score given when enemy is destroyed

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!PhotonNetwork.IsMasterClient) return; // ✅ Only Master Client modifies health

        health -= damage;

        // ✅ Show floating damage text using UIManager
        if (UIManager.Instance != null)
        {
            UIManager.Instance.photonView.RPC("ShowDamageText", RpcTarget.All, transform.position, damage);
        }

        if (health <= 0)
        {
            // ✅ Increase the player's score
            UIManager.Instance?.AddScore(scoreValue);

            // ✅ Destroy the enemy across all clients
            PhotonNetwork.Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")) // ✅ Ensure bullet has correct tag
        {
            TakeDamage(10); // ✅ Apply damage properly
            Destroy(collision.gameObject);
        }
    }
}
