using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
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

	private Transform player;
	private float totalYDistance; //  시작 지점과 마지막 지점 사이의 y 거리
	private float playerYDistance; // 시작 지점과 플레이어 사이의 y 거리
	private float percentage; // 시작 지점부터 플레이어까지의 y 거리 백분율
	private int score; 
	private int bestScore; // 최고점 임시 저장

	bool gameStart = false;

	private void Awake()
	{
		StartCoroutine(NetworkConnectCheckRoutine());
	}

	IEnumerator NetworkConnectCheckRoutine()
	{
		yield return new WaitUntil(() => { return PhotonNetwork.IsConnected; });

		scoreSlider.enabled = true;

		if (PhotonNetwork.IsConnected)
		{
			if (photonView.IsMine)
			{
				if (gameObject.name == "ScoreSliderBoy")
				{
					if (GameObject.Find("PlayerBoy(Clone)"))
					{
						player = GameObject.Find("PlayerBoy(Clone)").transform;
					}
				}
				else if (gameObject.name == "ScoreSliderGirl")
				{
					if (GameObject.Find("PlayerGirl(Clone)"))
					{
						player = GameObject.Find("PlayerGirl(Clone)").transform;
					}
				}				
			}

			yield break;
		}
	}

	private void Update()
	{
		if (player != null)
		{
			ScoreCalculate();
		}			
		else 
		{
			if (GameObject.Find("PlayerBoy(Clone)"))
			{
				player = GameObject.Find("PlayerBoy(Clone)").transform;
			}
			else if (GameObject.Find("PlayerGirl(Clone)"))
			{
				player = GameObject.Find("PlayerGirl(Clone)").transform;
			}
		}
	}

	private void ScoreCalculate()
	{
		
		totalYDistance = Mathf.Abs(endPoint.position.y - startPoint.position.y);

		playerYDistance = Mathf.Abs(player.position.y - startPoint.position.y);

		percentage = Mathf.Clamp((playerYDistance / totalYDistance) * 100f, 0f, 100f);

		// 백분율 값을 정수로 변환
		score = Mathf.RoundToInt(percentage);

		// 현재 점수가 최고 점수를 초과하면 최고 점수를 업데이트하고 PlayerPrefs에 저장
		if (score > bestScore)
		{
			bestScore = score;
			PlayerPrefs.SetInt("BestScore", bestScore);
			
			scoreSlider.value = score;
			Debug.Log("New Best Score: " + bestScore);
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