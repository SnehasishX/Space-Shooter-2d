using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // Reference to the player prefab (must be in Resources folder)

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

        if (!IsPrefabInResources("Player"))
        {
            Debug.LogError("❌ ERROR: Player prefab not found in Resources! Move the prefab to 'Assets/Resources/'.");
            return;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);

        if (player == null)
        {
            Debug.LogError("❌ ERROR: Failed to instantiate Player prefab.");
            return;
        }

        Debug.Log("✅ Player instantiated successfully at: " + spawnPosition);

        PhotonView playerPhotonView = player.GetComponent<PhotonView>();
        if (playerPhotonView == null)
        {
            Debug.LogError("❌ ERROR: PhotonView component missing on the Player prefab.");
            return;
        }

        StartCoroutine(AssignNicknameDelayed(playerPhotonView));
    }

    private System.Collections.IEnumerator AssignNicknameDelayed(PhotonView playerPhotonView)
    {
        yield return new WaitForSeconds(0.1f);

        if (playerPhotonView.Owner != null)
        {
            playerPhotonView.Owner.NickName = PhotonNetwork.NickName;
            Debug.Log("✅ Player assigned nickname: " + playerPhotonView.Owner.NickName);
        }
        else
        {
            Debug.LogError("❌ ERROR: Owner is still null after delay, cannot assign NickName.");
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-5f, 5f);
        float y = Random.Range(-5f, 5f);
        return new Vector3(x, y, 0);
    }

    private bool IsPrefabInResources(string prefabName)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabName);
        return prefab != null;
    }
}
