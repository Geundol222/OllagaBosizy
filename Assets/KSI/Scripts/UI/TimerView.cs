using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviourPunCallbacks //, IPunObservable
{
	[SerializeField] private float limitTime = 300f; // 제한 시간 5분
	private float remainLimitTime; // 남은 제한 시간

	private TMP_Text timerText;

	private void Awake()
	{
		timerText = GetComponent<TMP_Text>();

		// Photon Network 초기화
		//PhotonNetwork.ConnectUsingSettings();
	}

	private void Start()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			// 마스터 클라이언트인 경우 타이머를 시작하고 다른 클라이언트와 동기화
			photonView.RPC("CallDisplayTimerRPC", RpcTarget.All, limitTime);
		}
	}

	private void DisplayTimer(float second)
	{
		remainLimitTime = second;
		StartCoroutine(UpdateTimerRoutine());
	}

	[PunRPC]
	public void CallDisplayTimerRPC(float second)
	{
		// 현재 PhotonView를 통해 RPC를 호출
		photonView.RPC("DisplayTimer", RpcTarget.All, second);
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

	//public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	//{
	//	if (stream.IsWriting)
	//	{
	//		stream.SendNext(remainLimitTime);
	//	}
	//	else
	//	{
	//		remainLimitTime = (float)stream.ReceiveNext();
	//	}
	//}
}