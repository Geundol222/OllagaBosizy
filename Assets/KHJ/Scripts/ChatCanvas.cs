using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ChatCanvas : MonoBehaviour
{
    [SerializeField] TMP_InputField chatInputField;                 //채팅창의 inputField
    [SerializeField] TMP_Text chatMessage;                          //채팅로그에 들어갈 text
    [SerializeField] RectTransform chatObjectParent;                //채팅로그에 들어갈 text가 생겨야 될 위치
    [SerializeField] Scrollbar scrollbar;                           //채팅로그가 계속 맨 아래쪽을 향하게 해야되서 받아옴
    public PhotonView PV;                                           //포톤뷰


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnEnter();
        }
    }

    //유저가 채팅창에 적은 텍스트를 방 전체 인원에게 ChatRPC함수를 실행하도록 해주는 함수
    private void OnEnter()
    {
        if (chatInputField.text == "")
        {
            chatInputField.ActivateInputField();
            return;
        }
        string mes = chatInputField.text.Trim();
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + mes, PhotonNetwork.LocalPlayer);
        chatInputField.text = "";
        chatInputField.ActivateInputField();
    }

    //방을 나가면 채팅로그를 지우는 함수
    public void OutRoom()
    {
        for (int i = 0; i < chatObjectParent.childCount; i++)
        {
            Destroy(chatObjectParent.GetChild(i).gameObject);
        }
    }

    //방에 있는 상태에서 다른 유저가 들어올 시 실행되는 함수
    public void InOutRPC(string chat)
    {
        TMP_Text text;
        text = Instantiate(chatMessage, chatObjectParent.transform);
        text.text = chat;
        text.color = Color.yellow;
        scrollbar.value = 0;
    }

    //방에 있는 인원 전체에게 채팅창에 있는 text를 채팅창 로그에 text로 넣어주는 함수
    [PunRPC]
    void ChatRPC(string chat, Player player)
    {
        TMP_Text text;
        text = Instantiate(chatMessage, chatObjectParent.transform);
        text.text = chat;
        if (player.GetPlayerTeam() == PlayerTeam.Troller)
        {
            text.color = Color.blue;
        }
        else if (player.GetPlayerTeam() == PlayerTeam.Climber)
        {
            text.color = Color.red;
        }
        else
        {
            text.color = Color.black;
        }
        scrollbar.value = 0;
    }
}
