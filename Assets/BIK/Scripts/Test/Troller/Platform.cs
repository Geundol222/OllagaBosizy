using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Platform : MonoBehaviourPun,IPunObservable
{
    private TrollerPlayerController trollerPlayerController { get { return GameManager.TrollerData.trollerPlayerController; } set { GameManager.TrollerData.trollerPlayerController = value;  } }
    [SerializeField] private Color pointerOverColor;
    [SerializeField] bool isFirst;
    [SerializeField] bool isClickable;
    [SerializeField] private TMP_Text currentStateText;
    [SerializeField] private Slider countDownSlider;

    public bool IsClickable { get { return isClickable; } }
    private Renderer[] renderers;
    private int playerCount; 
    private Color pointerOutColor = Color.white;
    private UICloseArea closeArea;

    private SetTrapUI setTrapUI;
    public SetTrapUI _setTrapUI { get { return setTrapUI; } set { setTrapUI = value; } }

    private Debuff platformCurrentDebuff;
    public Debuff_State currentDebuffState {  get { return platformCurrentDebuff.state; } set { platformCurrentDebuff.state = value;  } }

    private Coroutine debuffCountDownCoroutine;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            Destroy(gameObject);

        }
        if (isFirst)
            isClickable = false;
        else
            isClickable = true;
        
        closeArea = GameObject.Find("UICloseArea").GetComponent<UICloseArea>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Start()
    {
        platformCurrentDebuff = GameManager.Debuff.CreateNoneStateDebuff();
    }

    [PunRPC]
    public void UpdateCurrentStateText()
    {
        string text = "";
        if(currentDebuffState != Debuff_State.None)
        {
            text = currentDebuffState.ToString();
        }
        currentStateText.text = text;
    }

    public void DebuffQueueEnqueue()
    {
        GameManager.Debuff.DebuffQueueEnqueue();
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

    public void ShowSetTrapUI(Platform platform)
    {
        setTrapUI = GameManager.UI.ShowInGameUI<SetTrapUI>("UI/SetTrapButton");
        setTrapUI.SetParentPlatform(platform);
        setTrapUI.SetTarget(transform);
        setTrapUI.SetOffset(new Vector3(200, 0));
    }

    public void HideSetTrapUI()
    {
        GameManager.UI.CloseInGameUI(setTrapUI);
    }
    /// <summary>
    /// 함정 설치 버튼 보여주기
    /// </summary>
    public void ShowSetTrapButton()
    {
        isClickable = false;

        //1. 클릭된 플랫폼을 트롤러 컨트롤러의 현재 플랫폼으로 설정
        SetCurrentPlatform();

        //1-2. 혹시 이전 플랫폼이 NULL이면 현재 플랫폼을 이전 플랫폼으로 설정
        if (GameManager.TrollerData.prevPlatform == null)
        {
            SetPrevPlatform();
        }

        //2. 현재 플랫폼과 이전 플랫폼이 다르다면 이전 플랫폼을 닫아줌.
        if (GameManager.TrollerData.currentPlatform != GameManager.TrollerData.prevPlatform)
        {
            GameManager.TrollerData.prevPlatform._setTrapUI.ExecuteSetTrapButtonClosing();
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

        Debug.Log("얘 호출");
        ShowSetTrapUI(this);
    }

    /// <summary>
    /// 함정 설치 버튼 숨기기
    /// </summary>
    public void HideSetTrapButton()
    {
        if (setTrapUI == null)
            return;

        isClickable = true;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonView.RPC("PlayerExitPlatform", RpcTarget.AllBufferedViaServer);
        }

        //SetTrapUI 스크립트 내부에서 HideSetTrapButton 시 CloseArea의 Platform을 초기화 해주는 구조로 바꿈 - 230801 02:19 AM  
        //ClearCloseAreaPlatform();
        HideSetTrapUI();
    }
      
    /// <summary>
    /// 플레이어가 발판에 닿았다. 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("플레이어 닿음");
        if (PhotonNetwork.IsConnectedAndReady)
        {
            // 디버프가 없으면 return
            if (platformCurrentDebuff == null || currentDebuffState == Debuff_State.None)
                return;

            // 현재 플랫폼에 설치된 함정을 실행하는
            if(currentDebuffState == Debuff_State.NoColider)
                platformCurrentDebuff.SetTrap(this);
            StartDebuffCountDown();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine)
            return;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            CallRPCFunction("PlayerEnteredPlatform");
            CallRPCFunction("SwitchRenderColorEnter");
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
            CallRPCFunction("PlayerExitPlatform");
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
        if (isClickable == false)
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
            CallRPCFunction("SetTrap");
        }
    }
      
    [PunRPC]
    public void SetTrap()
    {
        // 디버그 큐에서 하나 빼오고 Dequeue();
        // 현재 플랫폼의 디버프를 지정
        platformCurrentDebuff = (Debuff) GameManager.TrollerData.debuffQueue.Dequeue();
        // 텍스트 업데이트 
        CallRPCFunction("UpdateCurrentStateText");
        // NoCollider가 아니라면현재 플랫폼에 함정을 설치하는 함수 호출
        if (currentDebuffState != Debuff_State.NoColider)
            platformCurrentDebuff.SetTrap(this);
        // 디버프 슬롯에 랜덤 디버프 하나 추가해주기
        DebuffQueueEnqueue();
        // 함정리스트 갱신하기

        Debug.Log(platformCurrentDebuff.state);
    }

    [PunRPC]
    public void ClearTrap()
    {
        platformCurrentDebuff = GameManager.Debuff.CreateNoneStateDebuff();
        currentDebuffState = platformCurrentDebuff.state;
        CallRPCFunction("UpdateCurrentStateText");
        platformCurrentDebuff.SetTrap(this);
        countDownSlider.value = 1;
        countDownSlider.gameObject.SetActive(false);
        debuffCountDownCoroutine = null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((int)currentDebuffState);
        }
        else
        {
            currentDebuffState = (Debuff_State) stream.ReceiveNext();
        }
    }

    public void StartDebuffCountDown()
    { 
        countDownSlider.gameObject.SetActive(true);
        if (debuffCountDownCoroutine == null)
        {
            debuffCountDownCoroutine = StartCoroutine(CountDownCoroutine());
        }
    }

    public IEnumerator CountDownCoroutine()
    {
        float time = 0;
        while(currentDebuffState != Debuff_State.None)
        {
            yield return new WaitForSeconds(1f);
            countDownSlider.value -= 0.2f;
            time++;
            if(time >= 5)
            {
                CallRPCFunction("ClearTrap");
                yield return null;
            }
                     
        }
    }

    public void ClearDebuff()
    {
        if(playerCount != 0)
        {
            return;
        }
    }

    public void CallRPCFunction(string functionName)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonView.RPC(functionName, RpcTarget.AllBufferedViaServer);
        }
    }
}
