using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class Platform : MonoBehaviourPun
{
    [SerializeField] private Color pointerOverColor;
    [SerializeField] bool isFirst;
    [SerializeField] bool isClickable;

    public UnityEvent<bool> OnChangeState;

    public bool IsClickable { get { return isClickable; } }
    private Renderer[] renderers;
    private int playerCount;
    private Color pointerOutColor = Color.white;

    private void Awake()
    {
        if (isFirst)
            isClickable = false;
        else
            isClickable = true;

        renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine)
            return;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonView.RPC("PlayerEnteredPlatform", RpcTarget.AllBufferedViaServer);
            photonView.RPC("SwitchRenderColorEnter", RpcTarget.AllBufferedViaServer);
        }
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
        if (!photonView.IsMine)
            return;

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

    public void SwitchRenderColorEnter()
    {
        if (!photonView.IsMine)
            return;

        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
                renderer.material.color = pointerOverColor;
        }
    }

    public void SwitchRenderColorExit()
    {
        if (!photonView.IsMine)
            return;

        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
                renderer.material.color = pointerOutColor;
        }
    }
}
