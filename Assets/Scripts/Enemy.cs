using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 50;
    private int health;

    public GameObject floatingDamagePrefab; // Assign in Inspector

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // Show floating damage
        ShowFloatingDamage(damage);

        // Reduce health
        health -= damage;

        // Check for death
        if (health <= 0)
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateScore(10);
            }
            
            // Inform spawner to remove this enemy from the list
            EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
            if (spawner != null)
            {
                spawner.RemoveEnemy(gameObject);
            }

            Destroy(gameObject);
        }
    }

    void ShowFloatingDamage(int damage)
    {
        if (floatingDamagePrefab == null) return;

        // Spawn floating damage at enemy position
        GameObject damageText = Instantiate(floatingDamagePrefab, transform.position, Quaternion.identity);

        // Set damage text
        FloatingDamage floatingDamage = damageText.GetComponent<FloatingDamage>();
        if (floatingDamage != null)
        {
            floatingDamage.SetDamageText(damage);
        }
    }
}
