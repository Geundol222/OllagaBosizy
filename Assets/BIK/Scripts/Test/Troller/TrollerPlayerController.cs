using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrollerPlayerController : MonoBehaviourPun
{
    [SerializeField] int debuffQueueLength;
    [SerializeField] int debuffCount;
    [SerializeField] public TMP_Text[] TrapListTexts;
    public Debuff Original_Debuff;
    public Queue<IDebuff> debuffQueue;

    private Platform currentPlatform;
    public Platform _currentPlatform { get { return currentPlatform; } }
    private Platform prevPlatform;
    public Platform _prevPlatform { get { return prevPlatform; } }

    private GameObject[] allPlatformList;
    int platformList_index = 0;
    TrapListUI trapListUI;

    private void Awake()
    {
        Original_Debuff = new Debuff((int)Debuff_State.None);
        DebuffQueueInit();
    }

    private void Start()
    {
        allPlatformList = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject go in allPlatformList)
        {
            go.name = $"Platform_{platformList_index++}";
        }
    }

    public void UpdateTrapList()
    {
        Debuff[] debuffArray = new Debuff[debuffQueue.Count];
        debuffQueue.CopyTo(debuffArray, 0);
        trapListUI.UpdateList(debuffArray);
    }

    public void DebuffQueueInit()
    {
        debuffQueue = new Queue<IDebuff>();
        trapListUI = GameObject.Find("TrapList").GetComponent<TrapListUI>();

        debuffQueueLength = 4;

        for (int i = 0; i < debuffQueueLength; i++)
        {
            Debuff debuff = (Debuff)Original_Debuff.clone();
            debuff.SetState(Random.Range(1, (int)Debuff_State.Length));
            debuffQueue.Enqueue(debuff);
        }

        UpdateTrapList();
    }

    public void DebuffQueueEnqueue()
    {
        if (debuffQueue.Count >= debuffQueueLength)
            return;

        Debuff debuff = (Debuff) Original_Debuff.clone();
        debuff.SetState(Random.Range(1, (int)Debuff_State.Length));
        debuffQueue.Enqueue(debuff);

        UpdateTrapList();
    }
      
    public void ClearBothPlatform()
    {
        currentPlatform = null;
        prevPlatform = null;
    }

    public void ClearCurrentPlatform()
    {
        currentPlatform = null;
    }

    public void SetCurrentPlatform(Platform platform)
    {
        currentPlatform = platform;
    }

    public void SetPrevPlatform(Platform platform)
    {
        prevPlatform = platform;
    }
}
