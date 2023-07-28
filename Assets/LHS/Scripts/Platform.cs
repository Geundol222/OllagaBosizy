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
    private Renderer[] renderer;
    private int playerCount;
    private Color pointerOutColor = Color.white;

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
        {
            photonView.RPC("PlayerEnteredPlatform", RpcTarget.AllBufferedViaServer);
            SwitchRenderColorExit();
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
        foreach (Renderer renderer in renderer)
        {
            if (renderer != null)
                renderer.material.color = pointerOverColor;
        }
    }

    public void SwitchRenderColorExit()
    {
        foreach (Renderer renderer in renderer)
        {
            if (renderer != null)
                renderer.material.color = pointerOutColor;
        }
    }
}
