using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) // Only the owner can control their player
        {
            GetComponent<PlayerController>().enabled = false;
        }
    }
}
