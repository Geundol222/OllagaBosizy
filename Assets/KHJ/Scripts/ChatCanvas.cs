using Photon.Pun;
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
            return;
        }
        string mes = chatInputField.text.Trim();
        Debug.Log(mes);
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + mes);
        chatInputField.text = "";
    }

    public void OutRoom()
    {
        for (int i = 0; i < chatObjectParent.childCount; i++)
        {
            Destroy(chatObjectParent.GetChild(i).gameObject);
        }
    }

    [PunRPC]
    public void InOutRPC(string chat)
    {
        TMP_Text text;
        text = Instantiate(chatMessage, chatObjectParent.transform);
        text.text = chat;
        text.color = Color.yellow;
        scrollbar.value = 0;
    }

    [PunRPC]
    void ChatRPC(string chat)
    {
        if (chat == PhotonNetwork.NickName + " : ")
        {
            return;
        }
        TMP_Text text;
        text = Instantiate(chatMessage, chatObjectParent.transform);
        text.text = chat;
        scrollbar.value = 0;
    }
}
