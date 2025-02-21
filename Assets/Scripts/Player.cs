using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement; // 🔥 Needed for matchmaking

public class Player : MonoBehaviourPun
{
    public int maxHealth = 100;
    private int health;

    void Start()
    {
        health = maxHealth;
        UIManager.Instance?.UpdateHealth(health, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UIManager.Instance?.UpdateHealth(health, maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (photonView.IsMine) // 🔥 Only the local player should handle their own death
        {
            // Check if any players are left
            if (GameObject.FindGameObjectsWithTag("Player").Length <= 1)
            {
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene("MainMenu"); // 🔥 Replace with actual matchmaking scene name
            }

            PhotonNetwork.Destroy(gameObject); // 🔥 Destroy this player object
        }
    }
}
