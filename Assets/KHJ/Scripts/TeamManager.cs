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

    //유저의 팀을 알려주는 함수
    public PlayerTeam GetTeam()
    {
        return PhotonNetwork.LocalPlayer.GetPlayerTeam();
    }

    //유저가 팀을 초기화 해주는 함수
    public void LeaveTeam()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.Remove("Team");
        PhotonNetwork.LocalPlayer.LeaveCurrentTeam();
    }

    //유저의 팀을 선정해주는 함수
    public bool SetTeam(PlayerTeam team)
    {
        //유저의 팀이 없을 때만 실행이 가능하도록 제작
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

    //유저의 팀을 바꿔주는 함수
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

    //
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

    //유저의 팀값을 주는 함수
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