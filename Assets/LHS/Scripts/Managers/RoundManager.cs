using Photon.Pun;
using UnityEngine;

public enum Round { NONE, ROUND1, ROUND2 }

public enum Climber { None, Goblin, Ghost, Boy, Girl }

public class RoundManager : MonoBehaviourPun
{
    public void SetRound(Round round)
    {
        switch (round)
        {
            case Round.ROUND1:                
                RoundTwo();
                break;
            case Round.ROUND2:                
                EndGame();
                break;
            default:
                PhotonNetwork.CurrentRoom.SetCurrentRound(Round.NONE);
                break;
        }
    }

    private void RoundTwo()
    {
        Debug.Log("Round2");

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.SetCurrentRound(Round.ROUND2);

        PhotonNetwork.LocalPlayer.SetLoad(false);

        GameManager.Scene.LoadScene(Scene.TEMP);
    }

    private void EndGame()
    {
        Debug.Log("EndGame");

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.CustomProperties.Clear();

        GameManager.TrollerData._setTrapPlatforms.Clear();
        GameManager.Scene.LoadScene(Scene.SCORE);
    }

    public Round GetRound()
    {
        return PhotonNetwork.CurrentRoom.GetCurrentRound();
    }

    public void NextRound()
    {
        RoundChangeProcess();

        SetRound(GetRound());
    }

    private void RoundChangeProcess()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);

        if (GetRound() == Round.ROUND1)
        {
            if (GameManager.Team.GetTeam() == PlayerTeam.Troller)
                GameManager.Team.SwitchTeam(PlayerTeam.Climber);
            else if (GameManager.Team.GetTeam() == PlayerTeam.Climber)
                GameManager.Team.SwitchTeam(PlayerTeam.Troller);

            GameManager.Team.TeamSplit();
        }
        else
            PhotonNetwork.LocalPlayer.CustomProperties.Remove("Team");
    }
}
