using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    private int health;

    void Start()
    {
        health = maxHealth;
        UIManager.Instance.UpdateHealth(health, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UIManager.Instance.UpdateHealth(health, maxHealth);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}


