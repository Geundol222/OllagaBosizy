using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] RectTransform roomContent;
    [SerializeField] RoomEntry roomEntryPrefab;
    [SerializeField] GameObject createRoomPanel;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text maxPlayerText;
    public Animator createRoomAnim;
    private Animator anim;
    bool IsCreateRoom;
    bool IsCreateRoomPanalOpen;

    Dictionary<string, RoomInfo> roomDictionary;

    private void Awake()
    {
        roomDictionary = new Dictionary<string, RoomInfo>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        IsCreateRoom = false;
        createRoomPanel.SetActive(false);
        createRoomPanel.SetActive(false);
        anim.SetTrigger("IsIn");
        IsCreateRoomPanalOpen = false;
    }

    private void OnDisable()
    {
        roomDictionary.Clear();
    }

    public void OpenCreateRoomMenu()
    {
        if (!IsCreateRoomPanalOpen)
        {
            createRoomPanel.SetActive(true);
            createRoomAnim.SetTrigger("IsOpen");
        }
    }

    public void CloseCreateRoomMenu()
    {
        StartCoroutine(CloseCreateRoomMenuRoutine());
    }

    //방이름을 받거나 랜덤이름을 부여하며 방을 만드는 함수
    public void CreateRoomConfirm()
    {
        if (IsCreateRoom)
        {
            return;
        }
        IsCreateRoom = true;
        string roomName = roomNameInputField.text;
        Debug.Log(roomName);
        if (roomName == "")
            roomName = $"Room {Random.Range(1000, 10000)}";

        int maxPlayer = 4;

        maxPlayer = Mathf.Clamp(maxPlayer, 2, 8);

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayer };
        Debug.Log(roomName);
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void Logout()
    {
        PhotonNetwork.Disconnect();
    }

    public void CreateRoom()
    {
        StartCoroutine(CreateRoomRoutine());
    }

    //룸리스트를 업데이트 해주는 함수
    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomContent.childCount; i++)
        {
            Destroy(roomContent.GetChild(i).gameObject);
        }

        foreach (RoomInfo info in roomList)
        {
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (roomDictionary.ContainsKey(info.Name))
                {
                    roomDictionary.Remove(info.Name);
                }

                continue;
            }

            if (roomDictionary.ContainsKey(info.Name))
            {
                roomDictionary[info.Name] = info;
            }
            else
            {
                roomDictionary.Add(info.Name, info);
            }
        }

        foreach (RoomInfo info in roomDictionary.Values)
        {
            RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
            entry.SetRoomInfo(info);
        }
    }

    IEnumerator CloseCreateRoomMenuRoutine()
    {
        createRoomAnim.SetTrigger("IsClose");
        yield return new WaitForSeconds(0.5f);
        createRoomPanel.SetActive(false);
        yield break;
    }

    IEnumerator CreateRoomRoutine()
    {
        CloseCreateRoomMenu();
        anim.SetTrigger("IsOut");
        yield return new WaitForSeconds(1.5f);
        CreateRoomConfirm();
        yield break;
    }
}