using Photon.Pun;
using UnityEngine;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomCodeInput;
    public TMP_Text statusText;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connect to Photon servers
        statusText.text = "Connecting to server...";
        Debug.Log("🌐 Connecting to Photon...");
    }

    public override void OnConnectedToMaster()
    {
        statusText.text = "Connected! Ready to create or join a room.";
        Debug.Log("✅ Connected to Photon Master Server.");
    }

    public void CreateRoom()
    {
        string roomCode = Random.Range(1000, 9999).ToString(); // Generate a random 4-digit code
        PhotonNetwork.CreateRoom(roomCode);
        statusText.text = "Creating Room: " + roomCode;
        Debug.Log("🛠️ Creating Room: " + roomCode);
    }

    public void JoinRoom()
    {
        if (!string.IsNullOrEmpty(roomCodeInput.text))
        {
            PhotonNetwork.JoinRoom(roomCodeInput.text);
            statusText.text = "Joining Room: " + roomCodeInput.text;
            Debug.Log("🔗 Joining Room: " + roomCodeInput.text);
        }
        else
        {
            statusText.text = "Enter a valid room code!";
            Debug.LogWarning("⚠️ Room code is empty!");
        }
    }

    public override void OnJoinedRoom()
    {
        statusText.text = "Joined Room: " + PhotonNetwork.CurrentRoom.Name;
        Debug.Log("✅ Successfully joined room: " + PhotonNetwork.CurrentRoom.Name);

        // Load the GameScene AFTER joining a room
        PhotonNetwork.LoadLevel("GameScene");
        Debug.Log("🎮 Loading GameScene...");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        statusText.text = "Failed to join room: " + message;
        Debug.LogError("❌ Failed to join room: " + message);
    }
}
