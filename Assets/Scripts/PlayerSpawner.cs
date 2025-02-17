using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public GameObject playerPrefab; // Assign in Inspector

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnPlayer(OwnerClientId);
        }
    }

    void SpawnPlayer(ulong clientId)
    {
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }
}
