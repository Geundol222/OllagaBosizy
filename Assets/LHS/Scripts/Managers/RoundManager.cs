using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public enum Round { NONE, ROUND1, ROUND2 }

public enum Climber { None, Boy, Girl }

public class RoundManager : MonoBehaviourPun
{
    private int count;

    private void Awake()
    {
        count = 0;
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i].GetPlayerTeam() == PlayerTeam.Climber)
                {
                    photonView.RPC("SetClimber", RpcTarget.AllBuffered, PhotonNetwork.PlayerList[i], count++);
                }
                else
                    PhotonNetwork.PlayerList[i].SetClimber(Climber.None);
            }
        }
    }

    public void SetRound(Round round)
    {
        switch (round)
        {
            case Round.NONE:
                RoundOne();
                break;
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

    private void RoundOne()
    {
        Debug.Log("Round1");

        PhotonNetwork.CurrentRoom.SetCurrentRound(Round.ROUND1);
    }

    private void RoundTwo()
    {
        Debug.Log("Round2");

        PhotonNetwork.LocalPlayer.SetLoad(false);
        PhotonNetwork.LocalPlayer.CustomProperties.Remove("Climber");
        PhotonNetwork.CurrentRoom.CustomProperties.Clear();

        PhotonNetwork.CurrentRoom.SetCurrentRound(Round.ROUND2);

        GameManager.Scene.LoadScene(Scene.GAME);
    }

    private void EndGame()
    {
        Debug.Log("EndGame");

        PhotonNetwork.CurrentRoom.CustomProperties.Clear();

        PhotonNetwork.CurrentRoom.SetCurrentRound(Round.NONE);

        GameManager.Scene.LoadScene(Scene.LOBBY);
    }

    private void SwitchTeam()
    {

    }

    public Round GetRound()
    {
        return PhotonNetwork.CurrentRoom.GetCurrentRound();
    }

    public void NextRound()
    {
        SwitchTeam();

        SetRound(GetRound());
    }

    [PunRPC]
    public void SetClimber(Player player, int count)
    {
        if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            if (count == 0)
            {
                player.SetClimber(Climber.Boy);
            }
            else if (count == 1)
            {
                player.SetClimber(Climber.Girl);
            }
            else
                return;
        }
        
        //foreach (Player other in PhotonNetwork.PlayerList)
        //{
        //    if (other.ActorNumber == player.ActorNumber)
        //        continue;
        //    else
        //    {
        //        if (player.ActorNumber < other.ActorNumber)
        //            player.SetClimber(Climber.Boy);
        //        else
        //            player.SetClimber(Climber.Girl);
        //    }
        //}
    }
}
