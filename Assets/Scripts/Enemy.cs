using UnityEngine;
using Photon.Pun;

public class Enemy : MonoBehaviourPun
{
    public int maxHealth = 50;
    private int health;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!PhotonNetwork.IsMasterClient) return; // ✅ Only Master Client modifies health

        health -= damage;

        // ✅ Show floating damage text using RPC
        photonView.RPC("ShowDamageText", RpcTarget.All, transform.position, damage);

        if (health <= 0)
        {
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

    [PunRPC]
    void ShowDamageText(Vector3 position, int damage)
    {
        // ✅ Use RPC to call `ShowDamageText` on PlayerShooting
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PhotonView playerView = player.GetComponent<PhotonView>();
            if (playerView != null)
            {
                playerView.RPC("ShowDamageText", RpcTarget.All, position, damage);
            }
        }
    }
}
