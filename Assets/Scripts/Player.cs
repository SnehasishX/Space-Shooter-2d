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

        photonView.RPC("RPC_PlayerDied", RpcTarget.AllBuffered, photonView.ViewID);
    }

    [PunRPC]
    void RPC_PlayerDied(int viewID)
    {
        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null) PhotonNetwork.Destroy(pv.gameObject);

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
