using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ScoreResultManager : MonoBehaviourPunCallbacks
{
	[SerializeField] private TMP_Text teamAScoreText;
	[SerializeField] private TMP_Text teamBScoreText;
	[SerializeField] private TMP_Text resultTextTeamA;
	[SerializeField] private TMP_Text resultTextTeamB;

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
	private void UpdateScore()
	{
		
		if (currentRound == 1)
		{
			myScore = teamAScore; // 첫 번째 경기 팀의 점수를 사용
		}
		else if (currentRound == 2)
		{
			myScore = teamBScore; // 두 번째 경기 팀의 점수를 사용
		}

		if (photonView.IsMine)
		{
			string myTeam = ((PlayerTeam)PhotonNetwork.LocalPlayer.GetPlayerTeam()).ToString();
			if (myTeam == "TeamA")
			{
				teamAScore += myScore;
			}
			else if (myTeam == "TeamB")
			{
				teamBScore += myScore;
			}
		}

		UpdateScoreText();

		if (PhotonNetwork.IsMasterClient)
		{
			// 마스터 클라이언트에서 라운드 종료 후 승패를 판정하고 결과를 모든 플레이어에게 알림
			// 총 라운드 수에 따라 조정
			if (currentRound == 2)
			{
				int bestScoreTeamA = GetBestScoreByTeam(PlayerTeam.Troller);
				int bestScoreTeamB = GetBestScoreByTeam(PlayerTeam.Climber);

				// 팀 A와 팀 B의 점수 합을 비교하여 승패 결정
				if (bestScoreTeamA > bestScoreTeamB)
				{
					photonView.RPC("DeclareWinner", RpcTarget.All, "TeamA");
				}
				else if (bestScoreTeamB > bestScoreTeamA)
				{
					photonView.RPC("DeclareWinner", RpcTarget.All, "TeamB");
				}
				else
				{
					photonView.RPC("DeclareWinner", RpcTarget.All, "Draw");
				}
			}
		}
	}

	private int GetBestScoreByTeam(PlayerTeam team)
	{
		int bestScore = 0;

		foreach (Player player in PhotonNetwork.PlayerList)
		{
			if (player.GetPlayerTeam() == team)
			{
				int playerScore = player.GetScore();
				if (playerScore > bestScore)
				{
					bestScore = playerScore;
				}
			}
		}
		return bestScore;
	}

	// 승패를 알리는 RPC 함수
	[PunRPC]
	private void DeclareWinner(string winner)
	{
		if ((string)photonView.Owner.CustomProperties["Team"] == "TeamA")
		{
			resultTextTeamA.text = "WIN !";
			resultTextTeamB.text = "DEFEAT !";
		}
		else if ((string)photonView.Owner.CustomProperties["Team"] == "TeamB")
		{
			resultTextTeamA.text = "DEFEAT !";
			resultTextTeamB.text = "WIN !";
		}
	}

	private void UpdateScoreText()
	{
		teamAScoreText.text = "Team A : " + teamAScore;
		teamBScoreText.text = "Team B : " + teamBScore;
	}
}
