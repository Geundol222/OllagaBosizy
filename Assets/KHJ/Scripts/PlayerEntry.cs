using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerReady;
    [SerializeField] TeamManager teamManager;

    public void SetPlayer(Player player)
    {
        playerName.text = player.NickName;
        playerReady.text = player.GetReady() ? "준비 완료!" : "";
    }

    public void SetPlayerTrollerTeam()
    {
        teamManager.SetTeam(PlayerTeam.Troller);
    }

    public void SetPlayerClimberTeam()
    {
        teamManager.SetTeam(PlayerTeam.Climber);
    }

    public void ChangeCustomProperty(PhotonHashtable property)
    {
        if (property.TryGetValue(CustomProperty.READY, out object value))
        {
            bool ready = (bool)value;
            playerReady.text = ready ? "준비 완료!" : "";
        }
        else
        {
            playerReady.text = "";
        }
    }

    public void LeaveRoom()
    {
        teamManager.LeaveTeam();
    }

    public string GetTeam()
    {
        return PhotonNetwork.LocalPlayer.GetPhotonTeam().Name;
    }
}