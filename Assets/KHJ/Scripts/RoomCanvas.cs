using Photon.Pun;
using Photon.Realtime;
using System.Collections;
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
    PhotonView PV;
    PlayerEntry entry;
    Animator anim;
    bool isStart;

    private void Awake()
    {
        playerDictionary = new Dictionary<int, PlayerEntry>();
        aTeamDictionary = new Dictionary<int, PlayerEntry>();
        bTeamDictionary = new Dictionary<int, PlayerEntry>();
        PV = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
    }

    //방에 처음 들어오면 플레이어 리스트를 받아 방을 만들어줌
    private void OnEnable()
    {
        isStart = false;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player != PhotonNetwork.LocalPlayer)
            {
                //각 팀에 어느 유저가 들어가 있는지 판별을 유저의 팀으로 함
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
                //ateamdictionary에 2명이상일경우 들어올때 b팀으로 들어가도록 제작
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

    //켄버스가 내려갈때 나오는 함수
    private void OnDisable()
    {
        foreach (int actorNumber in playerDictionary.Keys)
        {
            Destroy(playerDictionary[actorNumber].gameObject);
        }
        playerDictionary.Clear();
        aTeamDictionary.Clear();
        bTeamDictionary.Clear();
    }

    //다른 유저가 나갈 시 그 유저에 해당하는 playerentry를 삭제함.
    public void PlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerDictionary[otherPlayer.ActorNumber].gameObject);
        playerDictionary.Remove(otherPlayer.ActorNumber);
        if(!aTeamDictionary.Remove(otherPlayer.ActorNumber))
            bTeamDictionary.Remove(otherPlayer.ActorNumber);
        AllPlayerReadyCheck();
    }

    //유저의 properties가 변경될떄마다 실행되는 함수
    public void PlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        playerDictionary[targetPlayer.ActorNumber].ChangeCustomProperty(changedProps);
        AllPlayerReadyCheck();
    }

    //방장이 바뀌었을 때 실행되는 함수
    public void MasterClientSwitched(Player newMasterClient)
    {
        AllPlayerReadyCheck();
    }

    //게임이 시작되면 실행되는 함수
    public void StartGame()
    {
        //이미 시작함수가 실행되었는데 다시 눌릴 수 있는 경우를 배제
        if (isStart)
        {
            return;
        }
        isStart = true;
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.SetCurrentRound(Round.NONE);

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        GameManager.Scene.LoadScene(Scene.LOADING);
    }

    //방을 나갈때 실행되는 함수. animation을 위해 코루틴으로 제작
    public void LeaveRoom()
    {
        StartCoroutine(LeaveRoomRoutine());
    }

    //플레이어 레디 버튼을 누르면 누른 본인만 레디 상태가 바뀌도록 하는 함수
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

    //플레이어가 모두 레디가 되면 방장에게 start버튼을 활성화 시켜주는 함수
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
        //방이 2대2로만 실행 되기 때문에 2대2일때만 체크하도록 제작
        if (readyCount == PhotonNetwork.PlayerList.Length && aTeamDictionary.Count == 2 && bTeamDictionary.Count == 2)
            startButton.gameObject.SetActive(true);
        else
            startButton.gameObject.SetActive(false);
    }

    //b팀에서 a팀으로 바꿔주는 함수
    public void SwitchTeamA()
    {
        if (PhotonNetwork.LocalPlayer.GetReady() || PhotonNetwork.LocalPlayer.GetPlayerTeam() == PlayerTeam.Troller)
        {
            return;
        }
        //방인원 전체에게 함수를 실행시켜 방을 동기화 시키도록 함.
        PV.RPC("PlayerRoomUpdate", RpcTarget.All, PhotonNetwork.LocalPlayer, true, true);
    }

    //b팀에서 a팀으로 바꿔주는 함수
    public void SwitchTeamB()
    {
        if (PhotonNetwork.LocalPlayer.GetReady() || PhotonNetwork.LocalPlayer.GetPlayerTeam() == PlayerTeam.Climber)
        {
            return;
        }
        PV.RPC("PlayerRoomUpdate", RpcTarget.All, PhotonNetwork.LocalPlayer, true, false);
    }

    
    IEnumerator LeaveRoomRoutine()
    {
        PlayerEntry playerEntry = playerDictionary[PhotonNetwork.LocalPlayer.ActorNumber];
        playerEntry.LeaveRoom();
        anim.SetTrigger("OutRoom");
        yield return new WaitForSeconds(1);
        PhotonNetwork.LeaveRoom();
        yield break;
    }

    //방업데이트를 해주는 함수, 본인이 아닌 유저가 들어오거나, 본인을 포함한 유저가 팀을 바꾸면 bool값으로 유저가 들어온 것을 실행하거나 팀을 바꾸는 것을 실행한다.
    [PunRPC]
    public void PlayerRoomUpdate(Player newPlayer, bool isSwitch, bool isAteamSwitch)
    {
        //스위치가 트루값이면 팀을 바꿔주는 함수를, 아니면 새로 들어오는 유저를 넣어주는 함수를 실행하도록 제작
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