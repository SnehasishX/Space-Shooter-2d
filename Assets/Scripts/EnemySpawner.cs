using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnRadius = 10f;
    public float spawnDelay = 2f;
    public int maxEnemies = 10; // Maximum enemies allowed at a time

    private List<GameObject> activeEnemies = new List<GameObject>(); // List to track enemies

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            if (player == null) yield break;

            // Check if we have reached the limit
            if (activeEnemies.Count >= maxEnemies)
            {
                yield return null; // Wait without spawning
                continue;
            }
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(player.position.x + randomOffset.x, player.position.y + randomOffset.y, 0);
        
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            activeEnemies.Add(enemy); // Track the enemy
        
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.enabled = false; // Temporarily disable AI
            }

            StartCoroutine(ActivateEnemy(enemy, enemyAI));
        }
    }

    IEnumerator ActivateEnemy(GameObject enemy, EnemyAI enemyAI)
    {
        yield return new WaitForSeconds(Random.Range(2f, 3f));

        if (enemy != null && enemyAI != null)
        {
            enemyAI.enabled = true;
            enemyAI.FindPlayer();
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }
}
