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

    private Player player;

    public void SetPlayer(Player player)
    {
        this.player = player;
        playerName.text = player.NickName;
        playerReady.text = player.GetReady() ? "준비 완료!" : "";
    }

    [System.Obsolete]
    public void SetPlayerRedTeam()
    {
        PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.red);
    }

    [System.Obsolete]
    public void SetPlayerBlueTeam()
    {
        PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.blue);
    }

    [System.Obsolete]
    public void ChangeCustomProperty(PhotonHashtable property)
    {
        if (property.TryGetValue(CustomProperty.READY, out object value))
        {
            Debug.Log(PhotonNetwork.LocalPlayer.GetTeam());
            bool ready = (bool)value;
            playerReady.text = ready ? "준비 완료!" : "";
        }
        else
        {
            playerReady.text = "";
        }
    }
}