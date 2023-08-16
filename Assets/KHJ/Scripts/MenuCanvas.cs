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
    //[SerializeField] GameObject peopleScrollView;
    //[SerializeField] Animator peopleAnimator;
    bool IsCreateRoom;

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
        Debug.Log("인에이블");
        anim.SetTrigger("IsIn");
    }

    private void OnDisable()
    {
        roomDictionary.Clear();
    }

    public void OpenCreateRoomMenu()
    {
        createRoomPanel.SetActive(true);
        createRoomAnim.SetTrigger("IsOpen");
    }

    public void CloseCreateRoomMenu()
    {
        StartCoroutine(CloseCreateRoomMenuRoutine());
    }

    public void CreateRoomConfirm()
    {
        if (IsCreateRoom)
        {
            return;
        }
        IsCreateRoom = true;
        string roomName = roomNameInputField.text;
        if (roomName == "")
            roomName = $"Room {Random.Range(1000, 10000)}";

        int maxPlayer = 4;
        /*if (maxPlayerText.text == "1 vs 1")
            maxPlayer = 2;
        else if (maxPlayerText.text == "2 vs 2")
            maxPlayer = 4;
        else if (maxPlayerText.text == "3 vs 3")
            maxPlayer = 6;
        else if (maxPlayerText.text == "4 vs 4")
            maxPlayer = 8;
        else
        {
            Debug.Log("인원을 선택해 주세요");
            return;
        }*/

        maxPlayer = Mathf.Clamp(maxPlayer, 2, 8);

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayer };
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

    /*public void ChooseOne()
    {
        maxPlayerText.text = "1 vs 1";
    }
    public void ChooseTwo()
    {
        maxPlayerText.text = "2 vs 2";
    }
    public void ChooseThree()
    {
        maxPlayerText.text = "3 vs 3";
    }
    public void ChooseFour()
    {
        maxPlayerText.text = "4 vs 4";
    }
    public void OpenPeopleChooseScroll()
    {
        peopleScrollView.SetActive(true);
        peopleAnimator.SetTrigger("IsOpen");
    }
    public void ClosePeopleChooseScroll()
    {
        StartCoroutine(ClosePeopleView());
    }

    IEnumerator ClosePeopleView()
    {
        while (true)
        {
            peopleAnimator.SetTrigger("IsClose");
            yield return new WaitForSeconds(0.4f);
            peopleScrollView.SetActive(false);
            yield break;
        }
    }
    public void CCCCC()
    {
        Debug.Log(maxPlayerText.text);
    }*/

    IEnumerator CloseCreateRoomMenuRoutine()
    {
        createRoomAnim.SetTrigger("IsClose");
        yield return new WaitForSeconds(0.5f);
        roomNameInputField.text = "";
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
