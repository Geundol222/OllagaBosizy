using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomCanvas : MonoBehaviour
{
    [SerializeField] RectTransform playerContent1;
    [SerializeField] RectTransform playerContent2;
    [SerializeField] Button startButton;
    [SerializeField] PlayerEntry playerEntryPrefab;
    [SerializeField] LogImage logImage;
    PlayerEntry entry;
    PlayerEntry otherEntry;

    private Dictionary<int, PlayerEntry> playerDictionary;

    private void Awake()
    {
        playerDictionary = new Dictionary<int, PlayerEntry>();
    }

    private void OnEnable()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (playerContent1.childCount < 2)
            {
                entry = Instantiate(playerEntryPrefab, playerContent1);
                entry.SetPlayerTrollerTeam();
            }
            else
            {
                entry = Instantiate(playerEntryPrefab, playerContent2);
                entry.SetPlayerClimberTeam();
            }
            entry.SetPlayer(player);
            playerDictionary.Add(player.ActorNumber, entry);
            PhotonNetwork.LocalPlayer.SetReady(false);
            PhotonNetwork.LocalPlayer.SetLoad(false);

            AllPlayerReadyCheck();
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }

    private void OnDisable()
    {
        foreach (int actorNumber in playerDictionary.Keys)
        {
            Destroy(playerDictionary[actorNumber].gameObject);
        }
        playerDictionary.Clear();

        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public void PlayerEnterRoom(Player newPlayer)
    {
        /*for (int i = 0; i < playerContent1.childCount; i++)
        {
            Destroy(playerContent1.GetChild(i).gameObject);
        }
        for (int i = 0; i < playerContent2.childCount; i++)
        {
            Destroy(playerContent2.GetChild(i).gameObject);
        }

        PlayerEntry entry1;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (playerContent1.childCount < 2)
            {
                entry1 = Instantiate(playerEntryPrefab, playerContent1);
                entry.SetPlayerTrollerTeam();
            }
            else
            {
                entry1 = Instantiate(playerEntryPrefab, playerContent2);
                entry1.SetPlayerClimberTeam();
            }
            entry.SetPlayer(player);
        }
        playerDictionary.Add(newPlayer.ActorNumber, entry);
        AllPlayerReadyCheck();*/
        PlayerEntry entry1;
        if (playerContent1.childCount < 2)
        {
            entry1 = Instantiate(playerEntryPrefab, playerContent1);
        }
        else
        {
            entry1 = Instantiate(playerEntryPrefab, playerContent2);
        }
        entry1.SetPlayer(newPlayer);
        playerDictionary.Add(newPlayer.ActorNumber, entry1);
        AllPlayerReadyCheck();
    }

    public void PlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerDictionary[otherPlayer.ActorNumber].gameObject);
        playerDictionary.Remove(otherPlayer.ActorNumber);
        AllPlayerReadyCheck();
    }

    public void PlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        playerDictionary[targetPlayer.ActorNumber].ChangeCustomProperty(changedProps);
        AllPlayerReadyCheck();
    }

    public void MasterClientSwitched(Player newMasterClient)
    {
        AllPlayerReadyCheck();
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("gggg");
    }

    public void LeaveRoom()
    {
        entry.LeaveRoom();
        PhotonNetwork.LeaveRoom();
    }

    public void PlayerReady()
    {
        if (PhotonNetwork.LocalPlayer.GetReady())
        {
            PhotonNetwork.LocalPlayer.SetReady(false);
        }
        else
        {
            PhotonNetwork.LocalPlayer.SetReady(true);
        }
    }

    private void AllPlayerReadyCheck()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(false);
            return;
        }

        int readyCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetReady())
                readyCount++;
        }

        if (readyCount == PhotonNetwork.PlayerList.Length)
            startButton.gameObject.SetActive(true);
        else
            startButton.gameObject.SetActive(false);
    }
    public void PlayerTeam()
    {
        logImage.gameObject.SetActive(true);
        logImage.SetText(entry.GetTeam());        
    }
}