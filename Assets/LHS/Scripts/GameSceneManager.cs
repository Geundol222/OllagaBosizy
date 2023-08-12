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
    [SerializeField] TMP_Text propsInfo;
    [SerializeField] TMP_Text infoText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] float countDownTimer;
    [SerializeField] float gameCountDown;
    [SerializeField] List<GameObject> climberSpawnPoints;
    [SerializeField] GameObject trollerSpawnPoint;
    [SerializeField] RoundManager round;

    string load = PhotonNetwork.LocalPlayer.GetLoad().ToString();
    string ready = PhotonNetwork.LocalPlayer.GetReady().ToString();
    string team = PhotonNetwork.LocalPlayer.GetPlayerTeam().ToString();
    string climber = PhotonNetwork.LocalPlayer.GetClimber().ToString();

    private void Start()
    {
        PhotonNetwork.LocalPlayer.SetLoad(true);

        //if (PhotonNetwork.InRoom)
        //{
        //    PhotonNetwork.LocalPlayer.SetLoad(true);
        //}
        //else
        //{
        //    infoText.text = "Debug Mode";
        //    PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {Random.Range(1000, 10000)}";
        //    PhotonNetwork.ConnectUsingSettings();
        //}
    }

    //public override void OnConnectedToMaster()
    //{
    //    RoomOptions options = new RoomOptions() { IsVisible = false };
    //    PhotonNetwork.JoinOrCreateRoom("DebugRoom123", options, TypedLobby.Default);
    //}

    //public override void OnCreateRoomFailed(short returnCode, string message)
    //{
    //    Debug.Log(message);
    //}

    //public override void OnJoinRoomFailed(short returnCode, string message)
    //{
    //    Debug.Log(message);
    //}

    //public override void OnJoinedRoom()
    //{
    //    StartCoroutine(DebugGameSetupDelay());
    //}

    //IEnumerator DebugGameSetupDelay()
    //{
    //    yield return new WaitForSeconds(1f);
    //    DebugGameStart();
    //}

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
        yield break;
    }

    private void GameStart()
    {
        if (PhotonNetwork.CurrentRoom.GetCurrentRound() == Round.NONE)
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.CurrentRoom.SetCurrentRound(Round.ROUND1);
        }

        propsInfo.text = $"Ready : {ready}\nLoad : {load}\nTeam : {team}\nClimber : {climber}";

        GameManager.Pool.InitPool();
        GameManager.UI.InitUI();
        GameManager.Sound.InitSound();
        GameManager.Sound.FadeInAudio();

        GameManager.TrollerData.Init();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player == PhotonNetwork.LocalPlayer)
            {
                if (player.GetPlayerTeam() == PlayerTeam.Troller)
                {
                    PhotonNetwork.Instantiate("Troller/TrollerController", trollerSpawnPoint.transform.position, trollerSpawnPoint.transform.rotation);
                }
                else if (player.GetPlayerTeam() == PlayerTeam.Climber)
                {
                    if (player.GetClimber() == Climber.Boy)
                        PhotonNetwork.Instantiate("Climber/PlayerBoy", climberSpawnPoints[0].transform.position, climberSpawnPoints[0].transform.rotation);
                    else if (player.GetClimber() == Climber.Girl)
                        PhotonNetwork.Instantiate("Climber/PlayerGirl", climberSpawnPoints[1].transform.position, climberSpawnPoints[1].transform.rotation);
                }
            }
        }

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.SetCountDownTime(PhotonNetwork.ServerTimestamp);
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
            timerText.color = Color.white;

            int remainLimitTime = (int)(gameCountDown - (PhotonNetwork.ServerTimestamp - loadTime) / 1000f);

            int minutes = Mathf.FloorToInt(remainLimitTime / 60);
            int seconds = Mathf.FloorToInt(remainLimitTime % 60);
            timerText.text = $"{minutes:00} : {seconds:00}";

            yield return new WaitForEndOfFrame();
        }

        TimeOut();
        yield break;
    }

    private void TimeOut()
    {
        timerText.text = "TIME OUT";
        timerText.color = Color.red;

        StartCoroutine(RoundChangeRoutine());
    }

    IEnumerator RoundChangeRoutine()
    {
        yield return new WaitForSeconds(1f);

        round.NextRound();

        yield break;
    }

    public void PlayerStepEndPoint()
    {
        photonView.RPC("StepEndPoint", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void StepEndPoint()
    {
        StartCoroutine(RoundChangeRoutine());
    }
}
