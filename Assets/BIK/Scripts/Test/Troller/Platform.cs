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
    public SetTrapUI _setTrapUI {  get { return setTrapUI;  } }

    private UICloseArea closeArea;

    private void Awake()
    {
        if (isFirst)
            isClickable = false;
        else
            isClickable = true;

        trollerPlayerController = GameObject.Find("TrollerController").GetComponent<TrollerPlayerController>(); // 추후 DataManager로 구현되어야함
        closeArea = GameObject.Find("UICloseArea").GetComponent<UICloseArea>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void InitToUICloseArea()
    {
        Debug.Log("InitToUICloseArea");
        closeArea.Init(this);
    }

    public void ClearCloseAreaPlatform()
    {
        closeArea.ClearPlatform();
    }

    public void ClearBothPlatform()
    {
        trollerPlayerController.ClearBothPlatform();
    }

    public void ClearCurrentPlatform()
    {
        trollerPlayerController.ClearCurrentPlatform();
    }

    public void SetCurrentPlatform()
    {
        trollerPlayerController.SetCurrentPlatform(this);
    }

    public void SetPrevPlatform()
    {
        trollerPlayerController.SetPrevPlatform(this);
    }

    public void ShowSetTrapButton()
    {
        if (!photonView.IsMine)
            return;

        //1. 클릭된 플랫폼을 트롤러 컨트롤러의 현재 플랫폼으로 설정
        SetCurrentPlatform(); 

        //1-2. 혹시 이전 플랫폼이 NULL이면 현재 플랫폼을 이전 플랫폼으로 설정
        if(trollerPlayerController._prevPlatform == null)
        {
            SetPrevPlatform();
        }

        //2. 현재 플랫폼과 이전 플랫폼이 다르다면 이전 플랫폼을 닫아줌.
        if(trollerPlayerController._currentPlatform != trollerPlayerController._prevPlatform)
        {
            trollerPlayerController._prevPlatform._setTrapUI.ExecuteSetTrapButtonClosing();
            //trollerPlayerController._prevPlatform.ClearCloseAreaPlatform();
        }

        //3. 현재 플랫폼을 NULL로 바꿔주고
        ClearCurrentPlatform();
        //4. 이 플랫폼은 이제 이전 플랫폼으로 ..
        SetPrevPlatform();

        InitToUICloseArea(); // 종료영역에게 클릭시 종료 되어야할 Platform 참조하게 


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

        // SetTrapUI에서의 코루틴 사용으로 뭔가 어긋나는 중 일단 UICloseArea 스크립트의 Plarform을 굳이 Null 주지 않고 새로운 Platform으로 Update 해주는 방식으로 진행하겠음.
        //ClearCloseAreaPlatform();
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

    [PunRPC]
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

    [PunRPC]
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
