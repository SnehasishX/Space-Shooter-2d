using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RadarMinimap : MonoBehaviour
{
    public Transform player;  // Reference to the player
    public Camera mainCamera; // Reference to the main camera
    public float radarRange = 50f;  // Range of radar
    public RectTransform radarUI;  // Radar UI panel
    public bool rotateWithPlayer = true; // Option to rotate radar with player
    public GameObject playerBlipPrefab; // Player's blip prefab
    public GameObject enemyBlipPrefab; // Enemy's blip prefab
    public List<string> enemyTags; // Customizable list of enemy tags
    
    private Dictionary<Transform, GameObject> blips = new Dictionary<Transform, GameObject>();
    private GameObject playerBlip;
    
    void Start()
    {
        // Instantiate player blip in center
        playerBlip = Instantiate(playerBlipPrefab, radarUI);
        playerBlip.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    void Update()
    {
        FindEnemies(); // Continuously find enemies
        UpdateBlips();
    }

    void FindEnemies()
    {
        foreach (string tag in enemyTags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

            if (PhotonNetwork.InRoom)
            {
                objects = GameObject.FindObjectsOfType<PhotonView>()
                            .Where(pv => pv.CompareTag(tag))
                            .Select(pv => pv.gameObject)
                            .ToArray();
            }
            
            foreach (GameObject obj in objects)
            {
                if (!blips.ContainsKey(obj.transform))
                {
                    AddBlip(obj.transform);
                }
            }
        }
    }

    void AddBlip(Transform target)
    {
        GameObject blip = Instantiate(enemyBlipPrefab, radarUI);
        blips[target] = blip;
    }

    void UpdateBlips()
    {
        foreach (var entry in new Dictionary<Transform, GameObject>(blips))
        {
            Transform target = entry.Key;
            GameObject blip = entry.Value;

            if (target == null)
            {
                Destroy(blip);
                blips.Remove(target);
                continue;
            }

            Vector3 offset = target.position - player.position;
            if (offset.magnitude > radarRange)
            {
                blip.SetActive(false);
            }
            else
            {
                blip.SetActive(true);
                
                Vector3 screenPos = mainCamera.WorldToViewportPoint(target.position);
                if (screenPos.z < 0)
                {
                    blip.SetActive(false);
                    continue;
                }
                
                // Convert screen position to radar UI coordinates
                Vector2 radarPos = new Vector2(
                    (screenPos.x - 0.5f) * radarUI.rect.width,
                    (screenPos.y - 0.5f) * radarUI.rect.height
                );
                
                if (rotateWithPlayer)
                {
                    float angle = -player.eulerAngles.y * Mathf.Deg2Rad;
                    radarPos = new Vector2(
                        radarPos.x * Mathf.Cos(angle) - radarPos.y * Mathf.Sin(angle),
                        radarPos.x * Mathf.Sin(angle) + radarPos.y * Mathf.Cos(angle)
                    );
                }
                
                if (radarPos.magnitude > radarUI.rect.width / 2)
                {
                    radarPos = radarPos.normalized * (radarUI.rect.width / 2 - 5); // Prevents clipping
                }
                
                blip.GetComponent<RectTransform>().anchoredPosition = radarPos;
            }
        }
    }
}