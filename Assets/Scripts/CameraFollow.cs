using UnityEngine;
using Photon.Pun;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    public Vector3 offset = new Vector3(0, 0, -10f); // Adjust if needed
    public float smoothSpeed = 5f;

    void Start()
    {
        if (PhotonNetwork.LocalPlayer != null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                PhotonView photonView = player.GetComponent<PhotonView>();
                if (photonView != null && photonView.IsMine)
                {
                    target = player.transform;
                    break;
                }
            }
        }

        if (target == null)
        {
            Debug.LogError("❌ CameraFollow: No local player found!");
            gameObject.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Keep camera rotation fixed
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
