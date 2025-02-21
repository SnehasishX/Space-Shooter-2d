using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public int maxHealth = 100;
    private int health;

    void Start()
    {
        health = maxHealth;

        // ✅ Only update UI for the local player
        if (photonView.IsMine)
        {
            UIManager.Instance?.UpdateHealth(health, maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!photonView.IsMine) return; // ✅ Only the local player processes damage

        health -= damage;

        // ✅ Update only the local UI
        UIManager.Instance?.UpdateHealth(health, maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (!photonView.IsMine) return; // ✅ Prevent affecting other players

        photonView.RPC("ReturnToMainMenu", RpcTarget.All);
        PhotonNetwork.Destroy(photonView);
    }

    [PunRPC]
    void ReturnToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu"); // 🔥 Make sure "MainMenu" is correct
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health); // ✅ Sync health across the network
        }
        else
        {
            health = (int)stream.ReceiveNext();
        }
    }
}
