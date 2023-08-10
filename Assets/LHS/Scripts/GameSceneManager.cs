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
    [SerializeField] TMP_Text timerText;
    [SerializeField] float countDownTimer;
    [SerializeField] float gameCountDown;
    [SerializeField] List<GameObject> playerSpawnPoints;
    [SerializeField] RoundManager round;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = false;

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
        PhotonNetwork.JoinOrCreateRoom("DebugRoom123", options, TypedLobby.Default);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
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
        Debug.Log("Player Load");
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
        Debug.Log("Room Load");
        if (propertiesThatChanged.ContainsKey("LoadTime"))
        {
            StartCoroutine(GameStartTimer());
        }

        if (propertiesThatChanged.ContainsKey("CountDownTime"))
        {
            StartCoroutine(UpdateTimerRoutine());
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
        if (round.GetRound() == Round.ROUND1)
            round.SetRound(round.GetRound());

        GameManager.TrollerData.Init();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player == PhotonNetwork.LocalPlayer)
            {
                if (player.GetPlayerTeam() == PlayerTeam.Troller)
                {
                    PhotonNetwork.Instantiate("Troller/TrollerController", playerSpawnPoints[player.GetPlayerNumber()].transform.position, playerSpawnPoints[player.GetPlayerNumber()].transform.rotation);
                }
                else if (player.GetPlayerTeam() == PlayerTeam.Climber)
                {
                    if (player.GetClimber() == Climber.Boy)
                        PhotonNetwork.Instantiate("Climber/PlayerBoy", playerSpawnPoints[player.GetPlayerNumber()].transform.position, playerSpawnPoints[player.GetPlayerNumber()].transform.rotation);
                    else if (player.GetClimber() == Climber.Girl)
                        PhotonNetwork.Instantiate("Climber/PlayerGirl", playerSpawnPoints[player.GetPlayerNumber()].transform.position, playerSpawnPoints[player.GetPlayerNumber()].transform.rotation);
                }
            }
        }

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.SetCountDownTime(PhotonNetwork.ServerTimestamp);
        else
            StartCoroutine(UpdateTimerRoutine());
    }

    private void DebugGameStart()
    {
        Debug.Log("Debug Start");

        int playerIndex = PhotonNetwork.LocalPlayer.GetPlayerNumber();

        if (GameManager.Team.GetTeam() == PlayerTeam.Troller)
        {
            PhotonNetwork.Instantiate("Player/TrollerController", playerSpawnPoints[playerIndex].transform.position, playerSpawnPoints[playerIndex].transform.rotation);
        }
        else if (GameManager.Team.GetTeam() == PlayerTeam.Climber)
        {
            PhotonNetwork.Instantiate("PlayerBoy", playerSpawnPoints[playerIndex].transform.position, playerSpawnPoints[playerIndex].transform.rotation);
        }

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.SetCountDownTime(PhotonNetwork.ServerTimestamp);
        else
            StartCoroutine(UpdateTimerRoutine());
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

    private IEnumerator UpdateTimerRoutine()
    {
        int loadTime = PhotonNetwork.CurrentRoom.GetCountDownTime();

        while (gameCountDown > (PhotonNetwork.ServerTimestamp - loadTime) / 1000f)
        {
            int remainLimitTime = (int)(gameCountDown - (PhotonNetwork.ServerTimestamp - loadTime) / 1000f);

            int minutes = Mathf.FloorToInt(remainLimitTime / 60);
            int seconds = Mathf.FloorToInt(remainLimitTime % 60);
            timerText.text = $"{minutes:00} : {seconds:00}";

            yield return new WaitForEndOfFrame();
        }

        TimeOut();
    }

    private void TimeOut()
    {
        timerText.text = "TIME OUT";
        timerText.color = Color.red;
    }

}
