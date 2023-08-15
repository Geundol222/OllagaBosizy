using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCountView : MonoBehaviourPunCallbacks, IPunObservable
{
	[Header("Transform")]
	[SerializeField] private Transform startPoint; // 시작 지점
	[SerializeField] private Transform endPoint; // 마지막 지점

	[Header("ScoreUI")]
	[SerializeField] private Slider scoreSlider;

	[Header("Player")]
	[SerializeField] Transform player;
	
	private float totalYDistance; //  시작 지점과 마지막 지점 사이의 y 거리
	private float playerYDistance; // 시작 지점과 플레이어 사이의 y 거리
	private float percentage; // 시작 지점부터 플레이어까지의 y 거리 백분율
	private int score; 
	private int bestScore; // 최고점 임시 저장

	private void Start()
	{
		
		StartCoroutine(NetworkConnectCheckRoutine());
		StartCoroutine(PlayerFindRoutine());

		// 초기화 시에 bestScore 값을 플레이어 Properties에 설정
		bestScore = 0;
		if (PhotonNetwork.IsConnectedAndReady)
		{
			PhotonNetwork.LocalPlayer.SetScore(bestScore);
		}
	}

	IEnumerator NetworkConnectCheckRoutine()
	{
		yield return new WaitUntil(() => { return PhotonNetwork.IsConnected; });

		scoreSlider.enabled = true;

		if (PhotonNetwork.IsConnected)
		{			
			yield break;
		}
	}

	IEnumerator PlayerFindRoutine()
	{
		yield return new WaitUntil(() => { return gameObject.name == "ScoreSliderBoy" ? GameObject.Find("PlayerBoy(Clone)") : GameObject.Find("PlayerGirl(Clone)"); });

		if (gameObject.name == "ScoreSliderBoy")
		{
			player = GameObject.Find("PlayerBoy(Clone)").transform;
		}
		else if (gameObject.name == "ScoreSliderGirl")
		{
			player = GameObject.Find("PlayerGirl(Clone)").transform;
		}

		yield break;
	}

	private void Update()
	{
		if (player != null)
		{
			ScoreCalculate();
			photonView.RPC("UpdateSliderValue", RpcTarget.OthersBuffered, score);
		}			
	}

	[PunRPC]
	private void UpdateSliderValue(int newScore)
	{
		scoreSlider.value = newScore;
	}

	private void ScoreCalculate()
	{		
		totalYDistance = Mathf.Abs(endPoint.position.y - startPoint.position.y);
		playerYDistance = Mathf.Abs(player.position.y - startPoint.position.y);
		percentage = Mathf.Clamp((playerYDistance / totalYDistance) * 100f, 0f, 100f);
		score = Mathf.RoundToInt(percentage);

		// 점수가 기존 bestScore를 초과할 경우, 플레이어의 Properties 업데이트
		int bestScore = PhotonNetwork.LocalPlayer.GetScore();
		if (score > bestScore)
		{
			bestScore = score;
			PhotonNetwork.LocalPlayer.SetScore(bestScore);

			scoreSlider.value = bestScore;
			Debug.Log("New Best Score : " + bestScore);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(scoreSlider.value);
		}
		else
		{
			this.scoreSlider.value = (float)stream.ReceiveNext();
		}
	}
}