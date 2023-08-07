using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    public Debuff Original_Debuff { get { return GameManager.TrollerData.Original_Debuff; } }
    
    int debuffQueueLength { get { return GameManager.TrollerData.debuffQueueLength; } set { GameManager.TrollerData.debuffQueueLength = value; } }
    int debuffCount { get { return GameManager.TrollerData.debuffCount; } }
    Queue<IDebuff> debuffQueue { get { return GameManager.TrollerData.debuffQueue; } set { GameManager.TrollerData.debuffQueue = value; } }

    [SerializeField] public TMP_Text[] TrapListTexts;
    TrapListUI trapListUI;

    public void ClimberStepOnPlatform(Platform platform)
    {
        if (platform.currentDebuffState != Debuff_State.None)
        {
            platform.StartDebuffCountDown();
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

        Debuff debuff = (Debuff)Original_Debuff.clone();
        debuff.SetState(Random.Range(1, (int)Debuff_State.Length));
        debuffQueue.Enqueue(debuff);

        UpdateTrapList();
    }

    public Debuff CreateNoneStateDebuff()
    {
        Debuff debuff = (Debuff)Original_Debuff.clone();
        debuff.SetState((int)Debuff_State.None);

        return debuff;
    }
}
