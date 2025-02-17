using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float maxTravelDistance = 20f;
    
    private Vector3 startPosition;
    private GameObject shooter; // Store the shooter

    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
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
        if (shooter == null) // Check if shooter was destroyed
        {
            Destroy(gameObject);
            return;
        }
        if (other.gameObject == shooter) return; // Ignore collision with shooter

        if (other.CompareTag("Enemy") && shooter.CompareTag("Player"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        
        if (other.CompareTag("Player") && shooter.CompareTag("Enemy"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
