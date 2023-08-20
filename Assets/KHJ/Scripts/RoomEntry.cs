using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text currentPlayer;
    [SerializeField] Button joinRoomButton;
    bool joinroom;
    private RoomInfo info;

    //방의 이름을 보여주는 함수
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        info = roomInfo;
        roomName.text = roomInfo.Name;
        currentPlayer.text = $"{roomInfo.PlayerCount} / {roomInfo.MaxPlayers}";
        joinRoomButton.interactable = roomInfo.PlayerCount < roomInfo.MaxPlayers;
    }

    public void JoinRoom()
    {
        if (!joinroom)
            PhotonNetwork.JoinRoom(info.Name);
        else
            return;
    }
}