using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomCanvas : MonoBehaviour
{
    public Dictionary<int, PlayerEntry> playerDictionary;
    public Dictionary<int, PlayerEntry> aTeamDictionary;
    public Dictionary<int, PlayerEntry> bTeamDictionary;
    [SerializeField] RectTransform playerContent1;
    [SerializeField] RectTransform playerContent2;
    [SerializeField] Button startButton;
    [SerializeField] PlayerEntry playerEntryPrefab;
    [SerializeField] LogImage logImage;
    [SerializeField] LobbyManager lobbyManager;
    public int ActorNum;
    PhotonView PV;
    PlayerEntry entry;
    bool isStart;

    private void Awake()
    {
        playerDictionary = new Dictionary<int, PlayerEntry>();
        aTeamDictionary = new Dictionary<int, PlayerEntry>();
        bTeamDictionary = new Dictionary<int, PlayerEntry>();
        PV = GetComponent<PhotonView>();
        isStart = false;
    }

    private void OnEnable()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player != PhotonNetwork.LocalPlayer)
            {
                if (player.GetPlayerTeam() == PlayerTeam.Troller)
                {
                    entry = Instantiate(playerEntryPrefab, playerContent1);
                    entry.SetPlayer(player);
                    aTeamDictionary.Add(player.ActorNumber, entry);
                }
                else
                {
                    entry = Instantiate(playerEntryPrefab, playerContent2);
                    entry.SetPlayer(player);
                    bTeamDictionary.Add(player.ActorNumber, entry);
                }
            }
            else
            {
                if (aTeamDictionary.Count < 2)
                {
                    entry = Instantiate(playerEntryPrefab, playerContent1);
                    entry.SetPlayer(player);
                    aTeamDictionary.Add(player.ActorNumber, entry);
                    entry.SetPlayerTrollerTeam();
                }
                else
                {
                    entry = Instantiate(playerEntryPrefab, playerContent2);
                    entry.SetPlayer(player);
                    bTeamDictionary.Add(player.ActorNumber, entry);
                    entry.SetPlayerClimberTeam();
                }
            }
            if (player == PhotonNetwork.LocalPlayer)
            {
                entry?.Sprite();
            }
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
        aTeamDictionary.Clear();
        bTeamDictionary.Clear();

        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public void PlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerDictionary[otherPlayer.ActorNumber].gameObject);
        playerDictionary.Remove(otherPlayer.ActorNumber);
        if(!aTeamDictionary.Remove(otherPlayer.ActorNumber))
            bTeamDictionary.Remove(otherPlayer.ActorNumber);
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
        if (isStart)
        {
            return;
        }
        isStart = true;
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.SetCurrentRound(Round.NONE);

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        GameManager.Scene.LoadScene(Scene.GAME);
    }

    public void LeaveRoom()
    {
        PlayerEntry playerEntry = playerDictionary[PhotonNetwork.LocalPlayer.ActorNumber];
        playerEntry.LeaveRoom();
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

        if (readyCount == PhotonNetwork.PlayerList.Length && aTeamDictionary.Count == 2 && bTeamDictionary.Count == 2)
            startButton.gameObject.SetActive(true);
        else
            startButton.gameObject.SetActive(false);
    }

    public void ShowPlayerTeam()
    {
        logImage.gameObject.SetActive(true);
        logImage.SetText(entry.GetTeam());
    }

    public void SwitchTeamA()
    {
        if (PhotonNetwork.LocalPlayer.GetReady() || PhotonNetwork.LocalPlayer.GetPlayerTeam() == PlayerTeam.Troller)
        {
            return;
        }
        PV.RPC("PlayerRoomUpdate", RpcTarget.All, PhotonNetwork.LocalPlayer, true, true);
    }

    public void SwitchTeamB()
    {
        if (PhotonNetwork.LocalPlayer.GetReady() || PhotonNetwork.LocalPlayer.GetPlayerTeam() == PlayerTeam.Climber)
        {
            return;
        }
        PV.RPC("PlayerRoomUpdate", RpcTarget.All, PhotonNetwork.LocalPlayer, true, false);
    }

    [PunRPC]
    public void PlayerRoomUpdate(Player newPlayer, bool isSwitch, bool isAteamSwitch)
    {
        if (!isSwitch)
        {
            PlayerEntry entry;
            if (aTeamDictionary.Count < 2)
            {
                entry = Instantiate(playerEntryPrefab, playerContent1);
                entry.SetPlayer(newPlayer);
                playerDictionary.Add(newPlayer.ActorNumber, entry);
                aTeamDictionary.Add(newPlayer.ActorNumber, entry);
            }
            else
            {
                entry = Instantiate(playerEntryPrefab, playerContent2);
                entry.SetPlayer(newPlayer);
                playerDictionary.Add(newPlayer.ActorNumber, entry);
                bTeamDictionary.Add(newPlayer.ActorNumber, entry);
            }
            AllPlayerReadyCheck();
        }
        else
        {
            Destroy(playerDictionary[newPlayer.ActorNumber].gameObject);
            playerDictionary.Remove(newPlayer.ActorNumber);
            if (!aTeamDictionary.Remove(newPlayer.ActorNumber))
                bTeamDictionary.Remove(newPlayer.ActorNumber);
            //PV.RPC("TeamRemove", RpcTarget.AllBuffered, newPlayer.ActorNumber);
            if (isAteamSwitch)
            {
                PlayerEntry entry2;
                entry2 = Instantiate(playerEntryPrefab, playerContent1);
                entry2.SetPlayer(newPlayer);
                playerDictionary.Add(newPlayer.ActorNumber, entry2);
                aTeamDictionary.Add(newPlayer.ActorNumber, entry2);
                if (newPlayer == PhotonNetwork.LocalPlayer)
                {
                    entry.SetPlayerTrollerTeam();
                    entry2?.Sprite();
                }
            }
            else
            {
                PlayerEntry entry2;
                entry2 = Instantiate(playerEntryPrefab, playerContent2);
                entry2.SetPlayer(newPlayer);
                playerDictionary.Add(newPlayer.ActorNumber, entry2);
                bTeamDictionary.Add(newPlayer.ActorNumber, entry2);
                if (newPlayer == PhotonNetwork.LocalPlayer)
                {
                    entry.SetPlayerClimberTeam();
                    entry2?.Sprite();
                }
            }
            AllPlayerReadyCheck();
        }
    }
}