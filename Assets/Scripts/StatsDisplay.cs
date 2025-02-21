using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class StatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI pingText;
    private float deltaTime = 0.0f;

    void Start()
    {
        StartCoroutine(UpdatePing());
    }

    void Update()
    {
        // Calculate FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = $"FPS: {Mathf.Round(fps)}";
    }

    IEnumerator UpdatePing()
    {
        while (true)
        {
            // Get the player's ping (Replace with your multiplayer system if needed)
            // int ping = PhotonNetwork.GetPing(); // Simulating ping (Replace with actual ping system)
            // pingText.text = $"Ping: {ping}ms";

            yield return new WaitForSeconds(1f); // Update every second
        }
    }
}
