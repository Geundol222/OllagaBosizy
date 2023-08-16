using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public enum PlayerTeam { None, Troller, Climber }

public class TeamManager : MonoBehaviourPun
{
    private int climberCount;
    private int trollerCount;

    private void Awake()
    {
        photonView.ViewID = 999;
    }

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