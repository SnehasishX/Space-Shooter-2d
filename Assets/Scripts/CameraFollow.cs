using UnityEngine;
using Photon.Pun;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    public Vector3 offset = new Vector3(0, 0, -10f);
    public float smoothSpeed = 5f;

    void Start()
    {
        FindLocalPlayer();
    }

    void LateUpdate()
    {
        if (target == null)
        {
            FindAnotherPlayer();
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

    void FindLocalPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                target = player.transform;
                return;
            }
        }
        Debug.LogWarning("❌ No local player found for the camera!");
    }

    void FindAnotherPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player != null)
            {
                target = player.transform;
                Debug.Log("🎥 Camera switched to another player!");
                return;
            }
        }
        
        Debug.LogWarning("⚠️ No players left! Camera stopped.");
        gameObject.SetActive(false); // Disable camera if no players are alive
    }
}
