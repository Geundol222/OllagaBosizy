using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public enum TeamState { Climber, Troller }

public class TeamManager : PhotonTeamsManager
{
    public PhotonTeam GetTeam(Player player)
    {
        return player.GetPhotonTeam();
    }

    public void SetTeam(Player player)
    {
        player.team
    }
}
