using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public enum Round { NONE, ROUND1, ROUND2 }

public enum Climber { None, Goblin, Ghost, Boy, Girl }

public class RoundManager : MonoBehaviourPun
{
    private int climberCount;
    private int trollerCount;

    private void Awake()
    {
        climberCount = 0;
        trollerCount = 0;
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (GetRound() == Round.NONE)
                SetRound(GetRound());

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i].GetPlayerTeam() == PlayerTeam.Climber)
                {
                    photonView.RPC("SetClimber", RpcTarget.AllBuffered, PhotonNetwork.PlayerList[i], climberCount++);
                }
                else if ((PhotonNetwork.PlayerList[i].GetPlayerTeam() == PlayerTeam.Troller))
                {
                    photonView.RPC("SetTroller", RpcTarget.AllBuffered, PhotonNetwork.PlayerList[i], trollerCount++);
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

        GameManager.Scene.LoadScene(Scene.GAME);
    }

    private void EndGame()
    {
        Debug.Log("EndGame");

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.CustomProperties.Clear();

        PhotonNetwork.LeaveRoom();

        GameManager.Scene.LoadScene(Scene.LOBBY);
    }

    public Round GetRound()
    {
        return PhotonNetwork.CurrentRoom.GetCurrentRound();
    }

    public void NextRound()
    {
        climberCount = 0;
        trollerCount = 0;

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
        }
        else
            PhotonNetwork.LocalPlayer.CustomProperties.Remove("Team");
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
    }

    [PunRPC]
    public void SetTroller(Player player, int count)
    {
        if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            if (count == 0)
            {
                player.SetClimber(Climber.Goblin);
            }
            else if (count == 1)
            {
                player.SetClimber(Climber.Ghost);
            }
            else
                return;
        }
    }
}
