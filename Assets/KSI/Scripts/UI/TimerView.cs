using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviourPunCallbacks
{
	[SerializeField] private float limitTime = 300f; // 제한 시간 5분
	private float remainLimitTime; // 남은 제한 시간

	private TMP_Text timerText;

	private void Awake()
	{
		timerText = GetComponent<TMP_Text>();
	}

	private void Start()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			// 마스터 클라이언트인 경우 타이머를 시작하고 다른 클라이언트와 동기화
			photonView.RPC("DisplayTimer", RpcTarget.AllBuffered, limitTime);
		}
	}

	[PunRPC]
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

			timerText.color = Color.white;
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
}