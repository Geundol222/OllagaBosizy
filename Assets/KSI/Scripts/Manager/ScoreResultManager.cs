using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
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

	private void Start()
	{
		GameManager.Sound.PlaySound("ScoreScene/bgm",Audio.BGM);

		teamAScore = 0;
		teamBScore = 0;

		UpdateScore();

		StartCoroutine(EndRoutine());
	}

	IEnumerator EndRoutine()
	{
		yield return new WaitForSeconds(10f);

		PhotonNetwork.LocalPlayer.CustomProperties.Clear();
		PhotonNetwork.LeaveRoom();
		
		GameManager.Scene.LoadScene(Scene.LOBBY);
	}

	// 플레이어의 점수를 업데이트하고 승패를 확인
	private void UpdateScore()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			foreach (Player player in PhotonNetwork.PlayerList)
			{
				if (player.GetPlayerTeam() == PlayerTeam.Troller)
				{
					teamBScore += player.GetScore();
				}
				else if (player.GetPlayerTeam() == PlayerTeam.Climber)
				{
					teamAScore += player.GetScore();
				}
			}

			photonView.RPC("DeclareWinner", RpcTarget.All, (teamAScore / 2), (teamBScore / 2));
		}
	}

	// 승패를 알리는 RPC 함수
	[PunRPC]
	private void DeclareWinner(int aScore, int bScore)
	{
		if (aScore > bScore)
		{
			resultTextTeamA.text = "WIN !";
			resultTextTeamB.text = "LOSE !";
		}
		else if (bScore > aScore)
		{
			resultTextTeamB.text = "WIN !";
			resultTextTeamA.text = "LOSE !";
		}
		else if (aScore == bScore)
		{
            resultTextTeamB.text = "DRAW !";
            resultTextTeamA.text = "DRAW !";
        }

		UpdateScoreText(aScore, bScore);
	}

	private void UpdateScoreText(int aScore, int bScore)
	{
		teamAScoreText.text = $"Team A : {aScore}";
		teamBScoreText.text = $"Team B : {bScore}";
	}
}
