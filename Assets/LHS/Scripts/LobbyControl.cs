using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyControl : MonoBehaviourPunCallbacks
{
    public void GoGameScene()
    {
        RoomOptions options = new RoomOptions() { IsVisible = false };
        PhotonNetwork.JoinOrCreateRoom("DebugRoom123", options, TypedLobby.Default);
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {Random.Range(1000, 10000)}";
            PhotonNetwork.ConnectUsingSettings();

            if (PhotonNetwork.IsConnected)
            {
                OnConnectedToMaster();
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
            GameManager.Scene.LoadScene(Scene.GAME);
    }
}
