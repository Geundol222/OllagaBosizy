using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, Menu, Room, SignUp, Found }

    [SerializeField] LoginCanvas loginCanvas;
    [SerializeField] MenuCanvas menuCanvas;
    [SerializeField] RoomCanvas roomCanvas;
    [SerializeField] SignUpCanvas signUpCanvas;
    [SerializeField] GameObject foundCanvas;

    public void Start()
    {
        if (PhotonNetwork.IsConnected)
            OnConnectedToMaster();
        else if (PhotonNetwork.InRoom)
            OnJoinedRoom();
        else if (PhotonNetwork.InLobby)
            OnJoinedLobby();
        else
            OnDisconnected(DisconnectCause.None);
    }

    public override void OnConnectedToMaster()
    {
        SetActivePanel(Panel.Menu);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SetActivePanel(Panel.Login);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
        Debug.Log($"Create room failed with error({returnCode}) : {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
        Debug.Log($"Join room failed with error({returnCode}) : {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
        Debug.Log($"Join random room failed with error({returnCode}) : {message}");
    }

    public override void OnJoinedRoom()
    {
        SetActivePanel(Panel.Room);
    }
    public void OnLoginCanvas()
    {
        SetActivePanel(Panel.Login);
    }

    public void OnFoundCanvas()
    {
        SetActivePanel(Panel.Found);
    }

    public override void OnLeftRoom()
    {
        SetActivePanel(Panel.Menu);
    }

    public void OnSignUpCanvas()
    {
        SetActivePanel(Panel.SignUp);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomCanvas.PlayerEnterRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        roomCanvas.PlayerLeftRoom(otherPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        roomCanvas.PlayerPropertiesUpdate(targetPlayer, changedProps);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        roomCanvas.MasterClientSwitched(newMasterClient);
    }

    public override void OnJoinedLobby()
    {
        SetActivePanel(Panel.Menu);
    }

    public override void OnLeftLobby()
    {
        SetActivePanel(Panel.Menu);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        menuCanvas.UpdateRoomList(roomList);
    }

    private void SetActivePanel(Panel panel)
    {
        if (loginCanvas != null) loginCanvas.gameObject.SetActive(panel == Panel.Login);
        if (menuCanvas != null) menuCanvas.gameObject.SetActive(panel == Panel.Menu);
        if (roomCanvas != null) roomCanvas.gameObject.SetActive(panel == Panel.Room);
        if (signUpCanvas != null) signUpCanvas.gameObject.SetActive(panel == Panel.SignUp);
        if (foundCanvas != null) foundCanvas.SetActive(panel == Panel.Found);
    }
}