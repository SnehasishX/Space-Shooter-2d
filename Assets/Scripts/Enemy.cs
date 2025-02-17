using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 50;
    private int health;

    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

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
}
