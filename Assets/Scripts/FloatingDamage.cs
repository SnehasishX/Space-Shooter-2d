using UnityEngine;
using TMPro;

public class FloatingDamage : MonoBehaviour
{
    private TextMeshPro damageText; // Change to TextMeshPro for World Space
    public float moveSpeed = 1f;
    public float fadeSpeed = 1f;
    public Vector3 moveDirection = new Vector3(0, 1, 0); // Moves upwards

    void Awake()
    {
        // Get TextMeshPro Component (World Space Text)
        damageText = GetComponent<TextMeshPro>();
        if (damageText == null)
        {
            Debug.LogError("No TextMeshPro found on FloatingDamage! Make sure it's attached.");
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
        if (damageText == null) return;

        // Move the text upwards smoothly
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Fade out the text
        damageText.alpha -= fadeSpeed * Time.deltaTime;

        // Destroy when fully faded
        if (damageText.alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
