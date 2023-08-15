using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSplitManager : MonoBehaviourPun
{
    private static TeamSplitManager split;

    public static TeamSplitManager Split { get { return split; } }

    private int climberCount;
    private int trollerCount;

    private void Start()
    {
        if (split != null)
        {
            Destroy(gameObject);
            return;
        }

        split = this;
        DontDestroyOnLoad(this);
    }

    public void TeamSplit()
    {
        climberCount = 0;
        trollerCount = 0;

        if (PhotonNetwork.IsMasterClient)
        {
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
