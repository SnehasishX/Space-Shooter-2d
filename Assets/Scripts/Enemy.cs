using UnityEngine;
using Photon.Pun;

public class Enemy : MonoBehaviourPun
{
    public int maxHealth = 50;
    private int health;
    public int scoreValue = 10;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        photonView.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(int damage)
    {
        health -= damage;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.photonView.RPC("ShowDamageText", RpcTarget.All, transform.position, damage);
        }

        if (health <= 0)
        {
            UIManager.Instance?.AddScore(scoreValue);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
