using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float maxTravelDistance = 20f; // Distance before destruction

    private Vector3 startPosition;

    void Start()
{
    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
}


    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

        // Destroy the projectile if it travels too far
        if (Vector3.Distance(startPosition, transform.position) >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Enemy"))
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    else if (other.CompareTag("Player"))
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
