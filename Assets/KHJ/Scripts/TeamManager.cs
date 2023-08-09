using Photon.Pun;
using Photon.Pun.UtilityScripts;
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
        PhotonNetwork.LocalPlayer.LeaveCurrentTeam();
        PhotonNetwork.LocalPlayer.CustomProperties.Remove(PhotonNetwork.LocalPlayer.GetPlayerTeam());
    }

    public void SetTeam(PlayerTeam team)
    {
        if (PhotonNetwork.LocalPlayer.GetPlayerTeam() == PlayerTeam.None)
        {
            if (PhotonNetwork.LocalPlayer.JoinTeam((byte)team))
                PhotonNetwork.LocalPlayer.SetPlayerTeam(team);
        }
    }
}
