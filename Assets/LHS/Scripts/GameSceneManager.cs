using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text infoText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] float countDownTimer;
    [SerializeField] float gameCountDown;
    [SerializeField] List<GameObject> climberSpawnPoints;
    [SerializeField] GameObject trollerSpawnPoint;
    [SerializeField] GameObject UICanvas;
    [SerializeField] RoundManager round;
    [SerializeField] GameSceneFadeUI gameSceneFadeUI;
    [SerializeField] Canvas UIcanvas;
    [SerializeField] Animator infoUIAnimator;

    string load = PhotonNetwork.LocalPlayer.GetLoad().ToString();
    string ready = PhotonNetwork.LocalPlayer.GetReady().ToString();
    string team = PhotonNetwork.LocalPlayer.GetPlayerTeam().ToString();
    string climber = PhotonNetwork.LocalPlayer.GetClimber().ToString();

    private PlayerController playerController;

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
        GameManager.Scene.LoadScene(Scene.LOBBY);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        GameManager.Scene.LoadScene(Scene.LOBBY);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {        
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

        PhotonNetwork.Instantiate("UI/RoundScene", new Vector3(-200, 0, 0), Quaternion.identity);

        while (countDownTimer > (PhotonNetwork.ServerTimestamp - loadTime) / 1000f)
        {
            int remainTime = (int)(countDownTimer - (PhotonNetwork.ServerTimestamp - loadTime) / 1000f);
            yield return new WaitForEndOfFrame();
        }
        infoText.text = "Game Start!";
        UICanvas.SetActive(true);
        GameStart();

        yield return new WaitForSeconds(1f);
        infoText.enabled = false;
        yield break;
    }

    private void GameStart()
    {
        if (PhotonNetwork.CurrentRoom.GetCurrentRound() == Round.NONE)
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.CurrentRoom.SetCurrentRound(Round.ROUND1);
        }

        GameManager.Sound.PlaySound("inGame/bgm", Audio.BGM);
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
                        playerController = PhotonNetwork.Instantiate("Climber/PlayerBoy", climberSpawnPoints[0].transform.position, climberSpawnPoints[0].transform.rotation).GetComponent<PlayerController>();
                    else if (player.GetClimber() == Climber.Girl)
                        playerController = PhotonNetwork.Instantiate("Climber/PlayerGirl", climberSpawnPoints[1].transform.position, climberSpawnPoints[1].transform.rotation).GetComponent<PlayerController>();
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
        StartCoroutine(RoundEndRoutine());

        yield return new WaitForSecondsRealtime(5f);

        gameSceneFadeUI.FadeOut();
        Time.timeScale = 1f;
        UICanvas.SetActive(false);
        round.NextRound();

        yield break;
    }

    IEnumerator RoundEndRoutine()
    {
        if (GameManager.Team.GetTeam() == PlayerTeam.Climber)
            playerController.SetPlayerScore();

        Time.timeScale = 0.01f;
        infoText.enabled = true;
        infoText.text = "Time's UP!";
        gameSceneFadeUI.TimeOut();
        GameManager.Sound.PlaySound("inGame/TimesUP");
        yield return new WaitForSecondsRealtime(1f);

        if (round.GetRound() == Round.ROUND1)
        {
            //infoText.text = "Let's Go To Next Round";
            //GameManager.Sound.PlaySound("inGame/RoundStartSlide");
        }
        else if (round.GetRound() == Round.ROUND2)
        {
            // TODO : 점수판 출력
        }

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
