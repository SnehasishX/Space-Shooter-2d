using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float maxTravelDistance = 20f;
    private Vector3 startPosition;
    public string shooterTag; // ✅ Store the shooter's tag (Player or Enemy)

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
        // ✅ Player bullets should hit only enemies
        if (shooterTag == "Player" && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject); // ✅ Destroy after hit
        }

        // ✅ Enemy bullets should hit only players
        if (shooterTag == "Enemy" && other.CompareTag("Player"))
        {
            PhotonView playerView = other.GetComponent<PhotonView>();

            if (playerView != null && playerView.IsMine) // ✅ Only the local player takes damage
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }

            Destroy(gameObject); // ✅ Destroy after hit
        }
    }
}
