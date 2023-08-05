using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class TimerViewTest : MonoBehaviourPunCallbacks
{

	[SerializeField] TMP_Text infoText;
	[SerializeField] float countDownTimer;

	[SerializeField] private float limitTime = 300f; // 제한 시간 5분
	private float remainLimitTime; // 남은 제한 시간

	private TMP_Text timerText;

	private void Awake()
	{
		timerText = GetComponent<TMP_Text>();
	}

	private void Start()
	{
		if (PhotonNetwork.InRoom)
		{
			PhotonNetwork.LocalPlayer.SetLoad(true);
		}
		else
		{
			infoText.text = "Degug Mode";
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

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log($"Disconnected : {cause}");
	}

	public override void OnLeftRoom()
	{
		Debug.Log("Left Room");
	}

	/* 방장(MasterClient) 권한 승계(Migration) */
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			Debug.Log("방장 바뀜");
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
	{
		if (changedProps.ContainsKey("Load"))
		{
			// 모든 플레이어 로딩 완료
			if (PlayerLoadCount() == PhotonNetwork.PlayerList.Length)
			{
				// 게임 시작
				Debug.Log($"All Player Loaded");
				infoText.text = $"All Player Loaded";

				// 방장만 서버 시간 설정
				if (PhotonNetwork.IsMasterClient)
					PhotonNetwork.CurrentRoom.SetLoadTime(PhotonNetwork.ServerTimestamp);
			}
			// 일부 플레이어 로딩 완료
			else
			{
				// 다른 플레이어 로딩 될 때 까지 대기
				Debug.Log($"Wait players {PlayerLoadCount()} / {PhotonNetwork.PlayerList.Length}");
				infoText.text = $"Wait players {PlayerLoadCount()} / {PhotonNetwork.PlayerList.Length}";
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
		Debug.Log("Game Start!");
		infoText.text = "Game Start!";
		GameStart();

		yield return new WaitForSeconds(1f);
		infoText.text = "";
	}

	private void DisplayTimer(float second)
	{
		remainLimitTime = second;
		StartCoroutine(UpdateTimerRoutine());
	}

	// 타이머 코루틴
	private IEnumerator UpdateTimerRoutine()
	{
		while (remainLimitTime >= 0)
		{
			int minutes = Mathf.FloorToInt(remainLimitTime / 60);
			int seconds = Mathf.FloorToInt(remainLimitTime % 60);
			timerText.text = $"{minutes:00} : {seconds:00}";

			remainLimitTime--;
			yield return new WaitForSeconds(1f);
		}

		TimeOut();
	}

	private void TimeOut()
	{
		timerText.text = "TIME OUT";
		timerText.color = Color.red;
	}

	private void GameStart()
	{
		Debug.Log("Normal Game Mode");
		// TODO : GameStart
	}

	// 1초 딜레이 코루틴
	IEnumerator DebugGameSetupDelay()
	{
		// 서버에게 여유 시간 1초 기다려주기
		yield return new WaitForSeconds(1f);
		DebugGameStart();
	}

	private void DebugGameStart()
	{
		Debug.Log("Debug Game Mode. IsMasterClient : " + PhotonNetwork.IsMasterClient);

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
}
