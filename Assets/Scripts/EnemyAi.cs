using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviourPun
{
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public float changeTargetTime = 5f;
    private Transform target;
    private List<GameObject> players = new List<GameObject>();
    private float switchTargetTimer;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient) // Only the Master Client controls enemy movement
        {
            FindPlayers();
            ChooseRandomTarget();
        }
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (target == null || !target.gameObject.activeInHierarchy)
        {
            ChooseRandomTarget();
        }

        if (target != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);
            if (distanceToPlayer <= detectionRange)
            {
                FollowPlayer();
                RotateTowardsPlayer();
            }
        }

        switchTargetTimer += Time.deltaTime;
        if (switchTargetTimer >= changeTargetTime)
        {
            ChooseRandomTarget();
            switchTargetTimer = 0f;
        }
    }

    void FindPlayers()
    {
        players.Clear();
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in playerObjects)
        {
            players.Add(player);
        }
    }

    void ChooseRandomTarget()
    {
        FindPlayers();
        if (players.Count > 0)
        {
            target = players[Random.Range(0, players.Count)].transform;
        }
    }

    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }
}
