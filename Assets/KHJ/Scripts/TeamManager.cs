using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerTeam { None, Troller, Climber }

public class TeamManager : MonoBehaviour
{
    public PlayerTeam GetTeam()
    {
        return PhotonNetwork.LocalPlayer.GetPlayerTeam();
    }

    public void LeaveTeam()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.Remove("Team");
        PhotonNetwork.LocalPlayer.LeaveCurrentTeam();
    }

    public bool SetTeam(PlayerTeam team)
    {
        if (PhotonNetwork.LocalPlayer.GetPlayerTeam() == PlayerTeam.None)
        {
            if (PhotonNetwork.LocalPlayer.JoinTeam((byte)team))
            {
                PhotonNetwork.LocalPlayer.SetPlayerTeam(team);
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public bool SwitchTeam(PlayerTeam team)
    {
        PhotonNetwork.LocalPlayer.CustomProperties.Remove("Team");

        if (PhotonNetwork.LocalPlayer.SwitchTeam((byte)team))
        {
            PhotonNetwork.LocalPlayer.SetPlayerTeam(team);
            return true;
        }
        else
        {
            return false;
        }
    }
}
