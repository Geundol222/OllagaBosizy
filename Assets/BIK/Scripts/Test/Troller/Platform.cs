using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class Platform : MonoBehaviourPun
{
    [SerializeField] private TrollerPlayerController trollerPlayerController;
    [SerializeField] private Color pointerOverColor;
    [SerializeField] bool isFirst;
    [SerializeField] bool isClickable;

    public bool IsClickable { get { return isClickable; } }
    private Renderer[] renderers;
    private int playerCount;
    private Color pointerOutColor = Color.white;

    private SetTrapUI setTrapUI;
    private SetTrapUI prev_SetTrapUI;

    private void Awake()
    {
        if (isFirst)
            isClickable = false;
        else
            isClickable = true;

        renderers = GetComponentsInChildren<Renderer>();
    }

    public void ShowSetTrapButton()
    {
        if (!photonView.IsMine)
            return;


        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonView.RPC("PlayerEnteredPlatform", RpcTarget.AllBufferedViaServer);
        }

        setTrapUI = GameManager.UI.ShowInGameUI<SetTrapUI>("UI/SetTrapButton");
        setTrapUI.SetParentPlatform(this);
        setTrapUI.SetTarget(transform);
        setTrapUI.SetOffset(new Vector3(200, 0));
    }

    public void HideSetTrapButton()
    {
        if (!photonView.IsMine)
            return;

        if (setTrapUI == null)
            return;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonView.RPC("PlayerExitPlatform", RpcTarget.AllBufferedViaServer);
        }

            GameManager.UI.CloseInGameUI(setTrapUI);
        
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
        if (!photonView.IsMine || isClickable == false)
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
