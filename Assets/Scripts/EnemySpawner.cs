using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPun
{
    public GameObject enemyPrefab;
    public float spawnRadius = 10f;
    public float spawnDelay = 2f;
    public int maxEnemies = 10; // Maximum enemies allowed at a time

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        if (PhotonNetwork.IsMasterClient) // Only Master Client spawns enemies
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            // Clean up any destroyed enemies
            activeEnemies.RemoveAll(enemy => enemy == null);

            // Check enemy limit
            if (activeEnemies.Count >= maxEnemies) continue;

            // Pick a random player as the target
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length == 0) continue;

            Transform targetPlayer = players[Random.Range(0, players.Length)].transform;
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(targetPlayer.position.x + randomOffset.x, targetPlayer.position.y + randomOffset.y, 0);

            // Spawn enemy across the network
            GameObject enemy = PhotonNetwork.Instantiate(enemyPrefab.name, spawnPosition, Quaternion.identity);
            activeEnemies.Add(enemy);
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
