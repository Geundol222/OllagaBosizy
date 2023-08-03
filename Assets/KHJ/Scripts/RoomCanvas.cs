using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomCanvas : MonoBehaviour
{
    [SerializeField] RectTransform playerContent1;
    [SerializeField] RectTransform playerContent2;
    [SerializeField] PlayerEntry playerEntryPrefab;
    [SerializeField] Button startButton;

    private Dictionary<int, PlayerEntry> playerDictionary;

    private void Awake()
    {
        playerDictionary = new Dictionary<int, PlayerEntry>();
    }

    private void OnEnable()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent1);
            entry.SetPlayer(player);
            playerDictionary.Add(player.ActorNumber, entry);
        }

        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);

        AllPlayerReadyCheck();
        PhotonNetwork.AutomaticallySyncScene = true;
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
        PlayerEntry entry;
        if (playerContent1.childCount < 2)
        {
            entry = Instantiate(playerEntryPrefab, playerContent1);
        }
        else
        {
            entry = Instantiate(playerEntryPrefab, playerContent2);
        }
        entry.SetPlayer(newPlayer);
        playerDictionary.Add(newPlayer.ActorNumber, entry);
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

        PhotonNetwork.LoadLevel("GameScene");
    }

    public void LeaveRoom()
    {
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
}