using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class Platform : MonoBehaviourPun
{
    public bool IsSteped {  get; private set; }
    private int playerCount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        photonView.RPC("PlayerEnteredPlatform", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void PlayerEnteredPlatform()
    {
        playerCount++;
        IsSteped = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        photonView.RPC("PlayerExitPlatform", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void PlayerExitPlatform()
    {
        playerCount--;
        IsSteped = false;
    }
}
