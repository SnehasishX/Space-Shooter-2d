using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPun
{
    public GameObject enemyPrefab;
    public float spawnRadius = 10f;
    public float spawnDelay = 2f;
    public int maxEnemies = 10;

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            activeEnemies.RemoveAll(enemy => enemy == null);
            if (activeEnemies.Count >= maxEnemies) continue;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length == 0) continue;

            Transform targetPlayer = players[Random.Range(0, players.Length)].transform;
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = targetPlayer.position + (Vector3)randomOffset;

            GameObject enemy = PhotonNetwork.Instantiate(enemyPrefab.name, spawnPosition, Quaternion.identity);
            activeEnemies.Add(enemy);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
    }
}
