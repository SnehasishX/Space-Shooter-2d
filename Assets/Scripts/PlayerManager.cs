using Photon.Pun;
using Photon.Realtime; // ✅ Ensure correct namespace is used
using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    private void Start()
    {
        Debug.Log("🚀 PlayerManager started in GameScene!");

        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogError("❌ ERROR: Not connected to Photon! Ensure you have joined a room before spawning players.");
            return;
        }

        if (PhotonNetwork.InRoom)
        {
            SpawnPlayer();
        }
        else
        {
            Debug.LogError("❌ ERROR: Not in a Photon Room! Waiting for room join...");
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("✅ OnJoinedRoom() triggered in GameScene! Spawning Player...");
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (!PhotonNetwork.InRoom)
        {
            Debug.LogError("❌ ERROR: Cannot spawn player. Not in a Photon Room.");
            return;
        }

        Vector3 spawnPosition = GetSafeSpawnPosition();
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);

        if (player == null)
        {
            Debug.LogError("❌ ERROR: Failed to instantiate Player prefab.");
            return;
        }

        Debug.Log("✅ Player instantiated successfully at: " + spawnPosition);
    }

    private Vector3 GetSafeSpawnPosition()
    {
        float x = Random.Range(-5f, 5f);
        float y = Random.Range(-5f, 5f);
        return new Vector3(x, y, 0);
    }

    // ✅ FIX: Explicitly use `Photon.Realtime.Player` to avoid conflicts
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (newMasterClient == null)
        {
            Debug.LogError("❌ ERROR: newMasterClient is NULL! Cannot transfer ownership.");
            return;
        }

        Debug.Log($"🔥 MasterClient switched! New Master: {newMasterClient.NickName}");

        if (PhotonNetwork.IsMasterClient)
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                PhotonView enemyView = enemy.GetComponent<PhotonView>();
                if (enemyView != null && !enemyView.IsMine)
                {
                    enemyView.TransferOwnership(newMasterClient);
                    Debug.Log($"✅ Transferred enemy {enemyView.ViewID} to {newMasterClient.NickName}");
                }
            }
        }
    }
}
