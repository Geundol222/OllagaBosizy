using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerReady;
    [SerializeField] TeamManager teamManager;

    private void Awake()
    {
        PhotonNetwork.LocalPlayer.LeaveCurrentTeam();
    }

    public void SetPlayer(Player player)
    {
        playerName.text = player.NickName;
        playerReady.text = player.GetReady() ? "준비 완료!" : "";
    }

    public void SetPlayerTrollerTeam()
    {
        teamManager.SetTeam(PlayerTeam.Climber);
    }

    public void SetPlayerClimberTeam()
    {
        teamManager.SetTeam(PlayerTeam.Troller);
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
}