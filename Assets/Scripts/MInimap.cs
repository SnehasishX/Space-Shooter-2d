using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Minimap : MonoBehaviour
{
    public RectTransform minimapPanel;  // UI Minimap Panel
    public Transform player;  // Main Player Transform
    public Camera minimapCamera;  // Minimap Camera
    public GameObject playerDotPrefab, enemyDotPrefab;  // Dot Prefabs
    public Transform dotContainer;  // UI Parent for Dots
    public float minimapScale = 0.1f;  // Scaling factor

    private Dictionary<Transform, GameObject> playerDots = new Dictionary<Transform, GameObject>();
    private Dictionary<Transform, GameObject> enemyDots = new Dictionary<Transform, GameObject>();

    void Update()
    {
        if (player == null) return;

        // Update Minimap Camera Position to Follow Player
        if (minimapCamera != null)
        {
            minimapCamera.transform.position = new Vector3(player.position.x, minimapCamera.transform.position.y, player.position.z);
        }

        UpdateDots("Player", playerDotPrefab, playerDots);
        UpdateDots("Enemy", enemyDotPrefab, enemyDots);
    }

    void UpdateDots(string tag, GameObject prefab, Dictionary<Transform, GameObject> dotDictionary)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        HashSet<Transform> currentObjects = new HashSet<Transform>();

        foreach (GameObject obj in objects)
        {
            Transform objTransform = obj.transform;
            if (obj.CompareTag("Crosshair") || obj.CompareTag("UI")) continue;

            currentObjects.Add(objTransform);
            
            if (!dotDictionary.ContainsKey(objTransform))
            {
                GameObject dot = Instantiate(prefab, dotContainer);
                dotDictionary[objTransform] = dot;
            }
            UpdateIcon(dotDictionary[objTransform].GetComponent<RectTransform>(), objTransform.position);
        }

        // Remove outdated dots
        List<Transform> toRemove = new List<Transform>();
        foreach (var key in dotDictionary.Keys)
        {
            if (!currentObjects.Contains(key)) toRemove.Add(key);
        }
        foreach (var key in toRemove)
        {
            Destroy(dotDictionary[key]);
            dotDictionary.Remove(key);
        }
    }

    private void UpdateIcon(RectTransform icon, Vector3 worldPosition)
    {
        float scale = minimapScale;
        float x = (worldPosition.x - player.position.x) * scale;
        float y = (worldPosition.z - player.position.z) * scale;

        icon.anchoredPosition = new Vector2(x, y);
    }
}
