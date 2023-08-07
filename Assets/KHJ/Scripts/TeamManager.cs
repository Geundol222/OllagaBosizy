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
    public PhotonTeam GetTeam()
    {
        return PhotonNetwork.LocalPlayer.GetPhotonTeam();
    }

    public void SetTeam(PlayerTeam team)
    {
        PhotonNetwork.LocalPlayer.JoinTeam((byte)team);
    }
}
