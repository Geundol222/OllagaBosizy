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
	[SerializeField] float gameCountDown;
	[SerializeField] List<GameObject> playerSpawnPoints;
	[SerializeField] private TextMeshProUGUI timerText;

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

	public override void OnConnected()
	{
		Debug.Log("OnConnected");
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("ConnectToMaster");
		RoomOptions options = new RoomOptions() { IsVisible = false };
		PhotonNetwork.JoinOrCreateRoom("DebugRoom", options, TypedLobby.Default);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.Log(message);
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom");
		StartCoroutine(DebugGameSetupDelay());
	}

	// 1초 딜레이 코루틴
	IEnumerator DebugGameSetupDelay()
	{
		// 서버에게 여유 시간 1초 기다려주기
		yield return new WaitForSeconds(1f);
		DebugGameStart();
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		Debug.Log(message);
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

		Debug.Log("Game Start!");
		infoText.text = "Game Start!";
		GameStart();

		yield return new WaitForSeconds(1f);
		infoText.text = "";
	}

	// 타이머 코루틴
	private IEnumerator UpdateTimerRoutine()
	{
		int loadTime = PhotonNetwork.CurrentRoom.GetCountDownTime();

		while (gameCountDown > (PhotonNetwork.ServerTimestamp - loadTime) / 1000f)
		{
			int remainLimitTime = (int)(gameCountDown - (PhotonNetwork.ServerTimestamp - loadTime) / 1000f);

			int minutes = Mathf.FloorToInt(remainLimitTime / 60);
			int seconds = Mathf.FloorToInt(remainLimitTime % 60);
			timerText.text = $"{minutes:00} : {seconds:00}";
			timerText.color = Color.white;

			yield return new WaitForEndOfFrame();
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

	private void DebugGameStart()
	{
		int playerIndex = PhotonNetwork.LocalPlayer.GetPlayerNumber();
		if (playerIndex == 0)
		{
			PhotonNetwork.Instantiate("PlayerBoy", playerSpawnPoints[playerIndex].transform.position, playerSpawnPoints[playerIndex].transform.rotation);
			Debug.Log("Player Boy Instantiate");
		}
		else if (playerIndex == 1)
		{
			PhotonNetwork.Instantiate("PlayerGirl", playerSpawnPoints[playerIndex].transform.position, playerSpawnPoints[playerIndex].transform.rotation);
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
}