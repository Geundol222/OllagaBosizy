using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

//서버의 함수들이 실행되면 그에따라 씬의 상태를 바꾸기 위해 제작된 스크립트
public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, Menu, Room }

    [SerializeField] LoginCanvas loginCanvas;
    [SerializeField] MenuCanvas menuCanvas;
    [SerializeField] RoomCanvas roomCanvas;
    [SerializeField] ChatCanvas chatCanvas;

    //처음 시작할 때 상태에 따라 다른 canvas가 열리도록 실행
    public void Start()
    {
        GameManager.Sound.PlaySound("MainLobbyRoom/bgm", Audio.BGM);
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.InRoom)
            {
                OnJoinedRoom();
                return;
            }
            else if (PhotonNetwork.InLobby)
            {
                OnJoinedLobby();
                return;
            }
            OnConnectedToMaster();
        }
        else
            OnDisconnected(DisconnectCause.None);
    }

    //서버에 연결 될 때 실행되는 함수
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        SetActivePanel(Panel.Menu);
    }

    //서버에서 나갈때 실행되는 함수
    public override void OnDisconnected(DisconnectCause cause)
    {
        SetActivePanel(Panel.Login);
    }

    //룸을 만드는 것을 실패했을때 실행되는 함수
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
    }

    //룸에 들어가는 것을 실패했을 때 실행되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
    }

    //룸에 들어갈 때 실행되는 함수
    public override void OnJoinedRoom()
    {
        SetActivePanel(Panel.Room);
        chatCanvas.OutRoom();
        PhotonNetwork.LocalPlayer.SetReady(false);
    }
    
    //로그인되엇을 때 실행되는 함수
    public void OnLoginCanvas()
    {
        SetActivePanel(Panel.Login);
    }

    //룸에서 나갈 때 실행되는 함수
    public override void OnLeftRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.JoinLobby();
    }

    //룸에 있을 때 다른 유저가 들어오면 실행되는 함수
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomCanvas.PlayerRoomUpdate(newPlayer, false, false);
        chatCanvas.InOutRPC(newPlayer.NickName + "님이 참가하셨습니다.");
    }

    //룸에 있을 때 다른 유저가 나가면 실행되는 함수
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        roomCanvas.PlayerLeftRoom(otherPlayer);
        chatCanvas.InOutRPC(otherPlayer.NickName + "님이 퇴장하셨습니다.");
    }

    //본인 혹은 그외에 플레이어의 properties가 변경되면 실행되는 함수
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        roomCanvas.PlayerPropertiesUpdate(targetPlayer, changedProps);
    }

    //방장이 바뀔 때 실행되는 함수
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        roomCanvas.MasterClientSwitched(newMasterClient);
    }

    //로비에 들어갈 때 실행되는 함수
    public override void OnJoinedLobby()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        SetActivePanel(Panel.Menu);
    }

    //로비에서 나갈 때 실행되는 함수, 로그아웃 할 때밖에 실행되지 않는다.
    public override void OnLeftLobby()
    {
        SetActivePanel(Panel.Login);
    }

    //로비에서 룸 리스트가 업데이트 될 때마다 실행되는 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        menuCanvas.UpdateRoomList(roomList);
    }

    //본인 상태에 따라 켄버스를 변경시켜주는 함수
    private void SetActivePanel(Panel panel)
    {
        if (loginCanvas != null) loginCanvas.gameObject.SetActive(panel == Panel.Login);
        if (menuCanvas != null) menuCanvas.gameObject.SetActive(panel == Panel.Menu);
        if (roomCanvas != null) roomCanvas.gameObject.SetActive(panel == Panel.Room);
    }

    public void PlayUIButtonClickSound()
    {
        GameManager.Sound.PlaySound("MainLobbyRoom/MouseClick");
    }

}