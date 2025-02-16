using UnityEngine;
using TMPro;

public class FloatingDamage : MonoBehaviour
{
    private TextMeshProUGUI damageText; // Private reference to prevent missing assignment errors
    public float moveSpeed = 1f;
    public float fadeSpeed = 1f;

    private void Awake()
    {
        // Automatically find TextMeshPro component attached to this GameObject
        damageText = GetComponent<TextMeshProUGUI>();

        if (damageText == null)
        {
            Debug.LogError("No TextMeshPro component found on FloatingDamage! Make sure the FloatingDamage prefab has a TextMeshProUGUI component.");
        }
    }

    public void SetDamageText(int damage)
    {
        if (damageText != null)
        {
            damageText.text = damage.ToString();
        }
    }

    void Update()
    {
        if (damageText == null) return; // Prevents errors if TextMeshPro is missing

        // Move the text upwards
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // Fade out the text
        Color color = damageText.color;
        color.a -= fadeSpeed * Time.deltaTime;
        damageText.color = color;

        // Destroy when fully faded
        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
