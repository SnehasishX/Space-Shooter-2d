using UnityEngine;
using Photon.Pun;

public class EnemyAI : MonoBehaviourPun
{
    public Transform player; 
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public float fireRate = 1.5f;
    private float nextFireTime;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient) // Only Master Client controls enemy movement
        {
            FindPlayer();
        }
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return; // Only the Master Client updates AI

        if (player == null)
        {
            FindPlayer();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            FollowPlayer();
            RotateTowardsPlayer();
        }
    }

    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }

    public void FindPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
        {
            player = players[Random.Range(0, players.Length)].transform; // Pick a random player in multiplayer
        }
    }
}
