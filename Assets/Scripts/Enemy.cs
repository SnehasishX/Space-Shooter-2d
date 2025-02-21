using UnityEngine;
using Photon.Pun;

public class Enemy : MonoBehaviourPun
{
    public int maxHealth = 50;
    private int health;
    public int scoreValue = 10;
    public float speed = 2f;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (!photonView.IsMine) return; // ✅ Only owner controls movement
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        // ✅ Ensure all clients send damage to the MasterClient
        photonView.RPC("RPC_TakeDamage", RpcTarget.MasterClient, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(int damage)
    {
        health -= damage;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.photonView.RPC("ShowEnemyDamageText", RpcTarget.All, transform.position, damage);
        }

        if (health <= 0)
        {
            HandleEnemyDestruction();
        }
    }

    void HandleEnemyDestruction()
    {
        if (photonView.IsMine)
        {
            UIManager.Instance?.AddScore(scoreValue);
            PhotonNetwork.Destroy(gameObject);
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
