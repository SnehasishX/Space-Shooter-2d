using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float maxTravelDistance = 20f;
    private Vector3 startPosition;
    public string shooterTag;

    public void SetShooterTag(string tag)
    {
        shooterTag = tag;
    }

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (shooterTag == "Player" && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        if (shooterTag == "Enemy" && other.CompareTag("Player"))
        {
            PhotonView playerView = other.GetComponent<PhotonView>();

            if (playerView != null)
            {
                playerView.RPC("RPC_TakeDamage", playerView.Owner, damage);
            }

            PhotonNetwork.Destroy(gameObject);
        }
    }
}
