using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class UIManager : MonoBehaviourPun
{
    public static UIManager Instance;

    public TextMeshProUGUI scoreText;
    public Slider healthBar;
    public GameObject floatingDamagePrefab; // ✅ Assign this in the Inspector

    private int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    [PunRPC]
    public void UpdateScore(int amount)
    {
        score += amount;
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void AddScore(int amount)
    {
        photonView.RPC("UpdateScore", RpcTarget.All, amount);
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    [PunRPC]
    public void ShowDamageText(Vector3 position, int damage)
    {
        if (floatingDamagePrefab == null)
        {
            Debug.LogError("❌ UIManager: Floating damage prefab is missing! Assign it in the Inspector.");
            return;
        }

        GameObject damageText = Instantiate(floatingDamagePrefab, position, Quaternion.identity);
        FloatingDamage floatingDamage = damageText.GetComponent<FloatingDamage>();

        if (floatingDamage != null)
        {
            floatingDamage.SetDamageText(damage);
        }
    }
}
