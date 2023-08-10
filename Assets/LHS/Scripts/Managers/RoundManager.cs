using Photon.Pun;
using Photon.Realtime;

public enum Round { ROUND1, ROUND2, END };

public class RoundManager : MonoBehaviourPun
{
    private int roundIndex = 0;

    public void SetRound(Round round)
    {
        switch (round)
        {
            case Round.ROUND1:
                RoundOne();
                break;
            case Round.ROUND2:
                RoundTwo();
                break;
            case Round.END:
                EndGame();
                break;
        }
    }

    private void RoundOne()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                int count = 0;
                if (PhotonNetwork.PlayerList[i].GetPlayerTeam() == PlayerTeam.Climber)
                {
                    photonView.RPC("SetClimber", RpcTarget.AllBufferedViaServer, PhotonNetwork.PlayerList[i], count++);
                }
            }
        }
    }

    private void RoundTwo()
    {
        PhotonNetwork.LocalPlayer.SetLoad(false);
        PhotonNetwork.LocalPlayer.CustomProperties.Remove(PhotonNetwork.LocalPlayer.GetClimber());
        PhotonNetwork.CurrentRoom.CustomProperties.Clear();

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i].GetPlayerTeam() == PlayerTeam.Climber)
                {
                    photonView.RPC("SetClimber", RpcTarget.AllBufferedViaServer, PhotonNetwork.PlayerList[i]);
                }
            }
        }

        GameManager.Scene.LoadScene(Scene.GAME);
    }

    private void EndGame()
    {
        roundIndex = 0;
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        PhotonNetwork.CurrentRoom.CustomProperties.Clear();
        GameManager.Scene.LoadScene(Scene.LOBBY);
    }

    private void SwitchTeam()
    {

    }

    public Round GetRound()
    {
        return (Round)roundIndex;
    }

    public void NextRound()
    {
        roundIndex++;

        SwitchTeam();
        SetRound(GetRound());
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
            else
            {
                player.SetClimber(Climber.Girl);
            }
        }
        
        //foreach (Player other in PhotonNetwork.PlayerList)
        //{
        //    if (other.ActorNumber == player.ActorNumber)
        //        continue;
        //    else
        //    {
        //        if (player.ActorNumber < other.ActorNumber)
        //            player.SetClimber(Climber.Boy);
        //        else
        //            player.SetClimber(Climber.Girl);
        //    }
        //}
    }
}
