using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 50; // Add this line
    private int health;

    public GameObject damageTextPrefab;

    void Start()
    {
        health = maxHealth; // Set health to max at start
    }

    public void TakeDamage(int damage)
{
    health -= damage;

    // ✅ Check if the prefab exists before using it
    if (damageTextPrefab != null)
    {
        GameObject dmgText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);

        // ✅ Check if FloatingDamage component exists
        FloatingDamage floatingText = dmgText.GetComponent<FloatingDamage>();
        if (floatingText != null)
        {
            floatingText.SetDamageText(damage);
        }
        else
        {
            Debug.LogError("FloatingDamage component is missing on damageTextPrefab!");
        }
    }
    else
    {
        Debug.LogError("damageTextPrefab is not assigned in the Inspector!");
    }

    if (health <= 0)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScore(10);
        }
        else
        {
            Debug.LogError("UIManager instance is NULL!");
        }

        Destroy(gameObject);
    }
}

}
