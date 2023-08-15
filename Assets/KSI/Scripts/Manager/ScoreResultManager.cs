using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreResultManager :MonoBehaviourPunCallbacks
{
	[SerializeField] private TMP_Text scoreText;
	[SerializeField] private TMP_Text resultText;
	private int teamAScore = 0;
	private int teamBScore = 0;
	private int currentRound = 1;
	private int myScore;

	private void Start()
	{
		teamAScore = 0;
		teamBScore = 0;
		currentRound = 1;

		UpdateScoreText();
	}

	// 플레이어의 점수를 업데이트하고 승패를 확인
	[System.Obsolete]
	private void UpdateScore()
	{
		myScore = (int)photonView.Owner.CustomProperties["ScoreRound" + currentRound.ToString()]; // 현재 라운드의 플레이어의 스코어

		if (photonView.IsMine)
		{
			string myTeam = (string)photonView.Owner.CustomProperties["Team"];
			if (myTeam == "blue")
			{
				teamAScore += myScore;
			}
			else if (myTeam == "red")
			{
				teamBScore += myScore;
			}
		}

		UpdateScoreText();

		if (PhotonNetwork.IsMasterClient)
		{
			// 마스터 클라이언트에서 라운드 종료 후 승패를 판정하고 결과를 모든 플레이어에게 알림
			if (currentRound == 2 && teamAScore + teamBScore >= 2) // 총 라운드 수에 따라 조정
			{
				if (teamAScore > teamBScore)
				{
					photonView.RPC("DeclareWinner", RpcTarget.All, "Team A");
				}
				else if (teamBScore > teamAScore)
				{
					photonView.RPC("DeclareWinner", RpcTarget.All, "Team B");
				}
				else
				{
					photonView.RPC("DeclareWinner", RpcTarget.All, "Draw");
				}
			}
		}
	}

	// 승패를 알리는 RPC 함수
	[PunRPC]
	private void DeclareWinner(string winner)
	{
		resultText.text = winner + " wins!"; // 승자 팀을 UI에 표시
		Debug.Log("Winner: " + winner);
	}

	private void UpdateScoreText()
	{
		scoreText.text = "Team A: " + teamAScore + " | Team B: " + teamBScore;
		Debug.Log("Team A: " + teamAScore + " | Team B: " + teamBScore);
	}
}
