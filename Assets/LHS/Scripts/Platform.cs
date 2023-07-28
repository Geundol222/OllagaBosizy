using Photon.Pun;
using UnityEngine;

public class Platform : MonoBehaviourPun
{
    [SerializeField] bool isFirst;
    [SerializeField] bool isClickable;

    public bool IsClickable { get { return isClickable; } }
    private int playerCount;

    private void Awake()
    {
        if (isFirst)
            isClickable = false;
        else
            isClickable = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsConnectedAndReady)
            photonView.RPC("PlayerEnteredPlatform", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    private void PlayerEnteredPlatform()
    {
        playerCount++;
        if (playerCount > 0)
            isClickable = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (PhotonNetwork.IsConnectedAndReady)
            photonView.RPC("PlayerExitPlatform", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    private void PlayerExitPlatform()
    {
        playerCount--;
        if (playerCount <= 0)
        {
            playerCount = 0;
            isClickable = true;
        }
    }
}
