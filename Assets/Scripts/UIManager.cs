using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class UIManager : MonoBehaviourPun
{
    public static UIManager Instance;

    public TextMeshProUGUI scoreText;
    public Slider healthBar;
    public GameObject floatingDamagePrefab;

    private int score = 0;
    public bool showPlayerDamage = false; // ✅ Toggle for player damage display

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Ensure PhotonView is assigned
            if (photonView == null)
            {
                Debug.LogError("❌ PhotonView is missing from UIManager!");
            }
            else
            {
                Debug.Log($"✅ UIManager PhotonView ID: {photonView.ViewID}");
            }

            DontDestroyOnLoad(gameObject); // ✅ Keep UIManager persistent
        }
        else
        {
            Debug.LogError("❌ Duplicate UIManager detected! Destroying...");
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
        if (photonView != null)
        {
            photonView.RPC("UpdateScore", RpcTarget.All, amount);
        }
        else
        {
            Debug.LogError("❌ UIManager PhotonView is missing!");
        }
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    [PunRPC]
    public void ShowEnemyDamageText(Vector3 position, int damage)
    {
        ShowFloatingDamage(position, damage);
    }

    [PunRPC]
    public void ShowPlayerDamageText(Vector3 position, int damage)
    {
        if (!showPlayerDamage) return;
        ShowFloatingDamage(position, damage);
    }

    private void ShowFloatingDamage(Vector3 position, int damage)
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
