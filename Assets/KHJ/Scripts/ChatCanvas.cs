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
using static UnityEngine.Rendering.DebugUI;

public class ChatCanvas : MonoBehaviour
{
    [SerializeField] TMP_InputField chatInputField;
    [SerializeField] TMP_Text chatMessage;
    [SerializeField] RectTransform chatObjectParent;
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] BaseEventData eventdata;
    public PhotonView PV;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnEnter();
        }
    }

    private void OnEnter()
    {
        if (chatInputField.text == "")
        {
            chatInputField.ActivateInputField();
            return;
        }
        string mes = chatInputField.text.Trim();
        Debug.Log(mes);
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + mes, PhotonNetwork.LocalPlayer);
        chatInputField.text = "";
        chatInputField.ActivateInputField();
    }

    public void OutRoom()
    {
        for (int i = 0; i < chatObjectParent.childCount; i++)
        {
            Destroy(chatObjectParent.GetChild(i).gameObject);
        }
    }

    public void GameStart()
    {
        PV.RPC("ChatRPC", RpcTarget.All, "게임을 시작합니다.");
        chatInputField.text = "";
    }

    public void InOutRPC(string chat)
    {
        TMP_Text text;
        text = Instantiate(chatMessage, chatObjectParent.transform);
        text.text = chat;
        text.color = Color.yellow;
        scrollbar.value = 0;
    }

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
