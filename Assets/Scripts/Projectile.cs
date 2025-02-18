using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float maxTravelDistance = 20f;

    public GameObject explosionEffectPrefab; // Optional: Assign an explosion prefab if needed

    private Vector3 startPosition;
    private GameObject shooter;
    private ParticleSystem particleSystem;
    private Vector3 originalScale; // Store original scale

    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
    }

    void Start()
    {
        startPosition = transform.position;
        particleSystem = GetComponentInChildren<ParticleSystem>();

        if (particleSystem != null)
        {
            originalScale = particleSystem.transform.localScale; // Store the scale before detaching
        }
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) >= maxTravelDistance)
        {
            DestroyProjectile();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (shooter == null) 
        {
            DestroyProjectile();
            return;
        }
        if (other.gameObject == shooter) return;

        if (other.CompareTag("Enemy") && shooter.CompareTag("Player"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                DestroyProjectile();
            }
        }

        if (other.CompareTag("Player") && shooter.CompareTag("Enemy"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                DestroyProjectile();
            }
        }
    }

    void DestroyProjectile()
    {
        if (particleSystem != null)
        {
            particleSystem.transform.parent = null; // Detach from projectile
            particleSystem.transform.localScale = originalScale; // Reapply original scale
            particleSystem.Stop(); // Stop emitting new particles
            Destroy(particleSystem.gameObject, particleSystem.main.duration); // Destroy after fading
        }

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Destroy the projectile itself
    }
}
