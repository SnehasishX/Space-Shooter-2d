using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MyPlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    public int maxHealth = 100;
    private int health;

    void Start()
    {
        health = maxHealth;

        if (photonView.IsMine)
        {
            UIManager.Instance?.UpdateHealth(health, maxHealth);
        }
    }

    [PunRPC]
    void RPC_TakeDamage(int damage)
    {
        health -= damage;
        UIManager.Instance?.UpdateHealth(health, maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_TakeDamage", RpcTarget.All, damage);
        }
    }

    void Die()
    {
        if (!photonView.IsMine) return;

        // 🔥 Ensure MasterClient can destroy if needed
        photonView.RPC("RPC_PlayerDied", RpcTarget.AllBuffered, photonView.ViewID);
    }

    [PunRPC]
    void RPC_PlayerDied(int viewID)
    {
        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null)
        {
            if (pv.IsMine)
            {
                PhotonNetwork.Destroy(pv.gameObject);
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                // ✅ Transfer ownership before destroying
                pv.TransferOwnership(PhotonNetwork.LocalPlayer);
                PhotonNetwork.Destroy(pv.gameObject);
            }
        }

        // ✅ Ensure only the local player loads MainMenu
        if (photonView.IsMine)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (int)stream.ReceiveNext();
        }
    }
}
