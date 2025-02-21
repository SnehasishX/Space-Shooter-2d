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

    [PunRPC]
    public void TakeDamage(int damage)
    {
        ShowFloatingDamage(damage);
        health -= damage;

        if (health <= 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                UIManager.Instance?.UpdateScore(10);
                EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
                spawner?.RemoveEnemy(gameObject);

                PhotonNetwork.Destroy(gameObject); // 🔥 Network-safe destruction
            }
        }
    }

    void ShowFloatingDamage(int damage)
    {
        if (floatingDamagePrefab == null) return;
        GameObject damageText = Instantiate(floatingDamagePrefab, transform.position, Quaternion.identity);
        FloatingDamage floatingDamage = damageText.GetComponent<FloatingDamage>();
        floatingDamage?.SetDamageText(damage);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            photonView.RPC("TakeDamage", RpcTarget.All, 10);
            Destroy(collision.gameObject);
        }
    }
}
