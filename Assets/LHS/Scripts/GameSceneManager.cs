using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text infoText;
    [SerializeField] float countDownTimer;
    [SerializeField] List<GameObject> playerSpawnPoints;

    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LocalPlayer.SetLoad(true);
        }
        else
        {
            infoText.text = "Debug Mode";
            PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {Random.Range(1000, 10000)}";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions() { IsVisible = false };
        PhotonNetwork.JoinOrCreateRoom("DebugRoom", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(DebugGameSetupDelay());
    }

    IEnumerator DebugGameSetupDelay()
    {
        yield return new WaitForSeconds(1f);
        DebugGameStart();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected : {cause}");
        // SceneManager.LoadScene("LobbyScene");
    }

    public override void OnLeftRoom()
    {
        // PhotonNetwork.LoadLevel("LobbyScene");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient) { }
            // TODO : 방장이 바꼈을 때
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if (changedProps.ContainsKey("Load"))
        {
            if (PlayerLoadCount() == PhotonNetwork.PlayerList.Length)
            {
                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.CurrentRoom.SetLoadTime(PhotonNetwork.ServerTimestamp);
            }
            else
            {
                Debug.Log($"Wait Players {PlayerLoadCount()} / {PhotonNetwork.PlayerList.Length}");
                infoText.text = $"Wait Players {PlayerLoadCount()} / {PhotonNetwork.PlayerList.Length}";
            }
        }
    }

    public override void OnRoomPropertiesUpdate(PhotonHashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("LoadTime"))
        {
            StartCoroutine(GameStartTimer());
        }
    }

    IEnumerator GameStartTimer()
    {
        int loadTime = PhotonNetwork.CurrentRoom.GetLoadTime();
        while (countDownTimer > (PhotonNetwork.ServerTimestamp - loadTime) / 1000f)
        {
            int remainTime = (int)(countDownTimer - (PhotonNetwork.ServerTimestamp - loadTime) / 1000f);
            infoText.text = $"All Player Loaded,\nStart CountDown : {remainTime + 1}";
            yield return new WaitForEndOfFrame();
        }
        infoText.text = "Game Start!";
        GameStart();

        yield return new WaitForSeconds(1f);
        infoText.text = "";
    }

    private void GameStart()
    {
        // TODO : GameStart
    }

    private void DebugGameStart()
    {
        int playerIndex = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        PhotonNetwork.Instantiate("PlayerBoy", playerSpawnPoints[playerIndex].transform.position, playerSpawnPoints[playerIndex].transform.rotation);
    }

    private int PlayerLoadCount()
    {
        int loadCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad())
                loadCount++;
        }
        return loadCount;
    }
}
