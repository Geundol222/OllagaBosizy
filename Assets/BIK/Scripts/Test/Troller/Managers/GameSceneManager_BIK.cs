using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class GameSceneManager_BIK : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text infoText;
    [SerializeField] float countDownTimer;

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
        // TODO : Debug GameStart
        // 방해자 컨트롤러 생성
        //StartCoroutine(StateLoadCoroutine());
        PhotonNetwork.Instantiate("Player/TrollerController", new Vector3(0, 0, 0), Quaternion.identity);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject("Stage/Platforms", new Vector3(0, 0, 0), Quaternion.identity);
        }
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
