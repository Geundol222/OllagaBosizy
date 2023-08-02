using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Platform : MonoBehaviourPun
{
    [SerializeField] private TrollerPlayerController trollerPlayerController;
    [SerializeField] private Color pointerOverColor;
    [SerializeField] bool isFirst;
    [SerializeField] bool isClickable;
    [SerializeField] private TMP_Text currentStateText;
    public bool IsClickable { get { return isClickable; } }
    private Renderer[] renderers;
    private int playerCount; 
    private Color pointerOutColor = Color.white;
    private SetTrapUI setTrapUI;
    public SetTrapUI _setTrapUI { get { return setTrapUI; } }
    private UICloseArea closeArea;

    private Debuff platformCurrentDebuff;
    private Debuff_State currentDebuffState;

    private void Awake()
    {
        currentDebuffState = Debuff_State.None;
        if (isFirst)
            isClickable = false;
        else
            isClickable = true;

        trollerPlayerController = GameObject.Find("TrollerController").GetComponent<TrollerPlayerController>(); // 추후 DataManager로 구현되어야함
        closeArea = GameObject.Find("UICloseArea").GetComponent<UICloseArea>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void UpdateCurrentStateText()
    {
        Debuff[] debuffArray = new Debuff[trollerPlayerController.debuffQueue.Count];
        trollerPlayerController.debuffQueue.CopyTo(debuffArray, 0);
        currentStateText.text = currentDebuffState.ToString();
        Debug.Log("-----------------------------------------");
        for(int i = 0; i < debuffArray.Length; i++)
        {
            Debug.Log(debuffArray[i].state.ToString());
        }
        Debug.Log("-----------------------------------------");
    }

    public void DebuffQueueEnqueue()
    {
        trollerPlayerController.DebuffQueueEnqueue();
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
        if (trollerPlayerController._prevPlatform == null)
        {
            SetPrevPlatform();
        }

        //2. 현재 플랫폼과 이전 플랫폼이 다르다면 이전 플랫폼을 닫아줌.
        if (trollerPlayerController._currentPlatform != trollerPlayerController._prevPlatform)
        {
            trollerPlayerController._prevPlatform._setTrapUI.ExecuteSetTrapButtonClosing();
            //SetTrapUI가 생성되어 Platform을 참조하면 스크립트 내부에서 CloseArea의 Platform을 초기화 해주는 구조로 바꿈 - 230801 02:19 AM 
            //trollerPlayerController._prevPlatform.ClearCloseAreaPlatform();
        }

        //3. 현재 플랫폼을 NULL로 바꿔주고
        ClearCurrentPlatform();
        //4. 이 플랫폼은 이제 이전 플랫폼으로 ..
        SetPrevPlatform();

        //SetTrapUI가 생성되어 Platform을 참조하면 스크립트 내부에서 CloseArea의 Platform을 초기화 해주는 구조로 바꿈 - 230801 02:19 AM 
        //InitToUICloseArea(); 

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

        //SetTrapUI 스크립트 내부에서 HideSetTrapButton 시 CloseArea의 Platform을 초기화 해주는 구조로 바꿈 - 230801 02:19 AM  
        //ClearCloseAreaPlatform();
        GameManager.UI.CloseInGameUI(setTrapUI);
    }

    /// <summary>
    /// 플레이어가 발판에 닿았다. 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            // 디버프가 없으면 return
            if (platformCurrentDebuff == null)
                return;

           

        }
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

    public void OnClickSetTrap()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonView.RPC("SetTrap",RpcTarget.AllBufferedViaServer);
        }
    }
      
    [PunRPC]
    public void SetTrap()
    {
        // 디버그 큐에서 하나 빼오고 Dequeue();
        // 현재 플랫폼의 디버프를 지정
        platformCurrentDebuff = (Debuff) trollerPlayerController.debuffQueue.Dequeue();
        // 현재 플랫폼의 DebuffState 업데이트 
        currentDebuffState = platformCurrentDebuff.state;
        // 텍스트 업데이트 
        UpdateCurrentStateText();
        // 현재 플랫폼에 함정을 설치하는 함수 호출
        platformCurrentDebuff.SetTrap(this);
        // 디버프 슬롯에 랜덤 디버프 하나 추가해주기
        DebuffQueueEnqueue();
        Debug.Log(platformCurrentDebuff.state);
    } 
       
}
