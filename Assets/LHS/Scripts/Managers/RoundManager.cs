using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public enum Round { ROUND1, ROUND2, END };

public class RoundManager : MonoBehaviour
{
    private int roundIndex = 0;

    private void SetRound(Round round)
    {
        switch (round)
        {
            case Round.ROUND2:
                RoundTwo();
                break;
            case Round.END:
                EndGame();
                break;
        }
    }

    private void RoundTwo()
    {
        SwitchTeam();
        PhotonNetwork.LocalPlayer.SetLoad(false);
        PhotonNetwork.CurrentRoom.CustomProperties.Clear();
        GameManager.Scene.LoadScene(Scene.GAME);
    }

    private void EndGame()
    {
        roundIndex = 0;
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        PhotonNetwork.CurrentRoom.CustomProperties.Clear();
        GameManager.Scene.LoadScene(Scene.LOBBY);
    }

    private void SwitchTeam()
    {

    }

    public Round GetRound()
    {
        return (Round)roundIndex;
    }

    public void NextRound()
    {
        roundIndex++;

        SetRound(GetRound());
    }
}
