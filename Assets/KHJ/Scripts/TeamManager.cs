using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum PlayerTeam {None, Troller, Climber }

public class TeamManager : MonoBehaviour
{
    public PlayerTeam GetTeam()
    {
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Code == 1)
            return PlayerTeam.Troller;
        else if (PhotonNetwork.LocalPlayer.GetPhotonTeam().Code == 2)
            return PlayerTeam.Climber;
        else return PlayerTeam.None;
    }

    public void LeaveTeam()
    {
        PhotonNetwork.LocalPlayer.LeaveCurrentTeam();
    }

    public void SetTeam(PlayerTeam team)
    {
        if (!PhotonNetwork.LocalPlayer.JoinTeam((byte)team))
        {
            PhotonNetwork.LocalPlayer.SwitchTeam((byte)team);
        }
        PhotonNetwork.LocalPlayer.JoinTeam((byte)team);
    }
}
