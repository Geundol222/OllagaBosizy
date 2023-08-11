using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public enum Round { ROUND1, ROUND2, END }

public enum Climber { None, Boy, Girl }

public class RoundManager : MonoBehaviourPun
{
    private int roundIndex;
    private int count;

    private void Awake()
    {
        roundIndex = 0;
        count = 0;
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i].GetPlayerTeam() == PlayerTeam.Climber)
                {
                    photonView.RPC("SetClimber", RpcTarget.AllBuffered, PhotonNetwork.PlayerList[i], count++);
                }
                else
                    PhotonNetwork.PlayerList[i].SetClimber(Climber.None);
            }
        }
    }

    public void SetRound(Round round)
    {
        switch (round)
        {
            case Round.ROUND2:
                RoundTwo();
                break;
            case Round.END:
                EndGame();
                break;
        }
    }

    private void RoundTwo()
    {
        PhotonNetwork.LocalPlayer.SetLoad(false);
        PhotonNetwork.LocalPlayer.CustomProperties.Remove(PhotonNetwork.LocalPlayer.GetClimber());
        PhotonNetwork.CurrentRoom.CustomProperties.Clear();

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
            else if (count == 1)
            {
                player.SetClimber(Climber.Girl);
            }
            else
                return;
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
