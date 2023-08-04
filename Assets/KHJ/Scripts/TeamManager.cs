using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TeamManager : MonoBehaviour
{
    public enum Team { Troller, Climber , None}

    [System.Obsolete]
    public Team GetTeam()
    {
        if (PhotonNetwork.LocalPlayer.GetTeam() == PunTeams.Team.red)
        {
            return Team.Troller;
        }
        else
        {
            return Team.Climber;
        }
    }

    [System.Obsolete]
    public void SetTeam(Team team)
    {
        if (team == Team.Troller)
        {
            PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.red);
        }
        else
        {
            PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.blue);
        }
    }
}
