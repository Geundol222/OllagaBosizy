using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{   
    public Debuff Original_Debuff { get { return GameManager.TrollerData.Original_Debuff; } } // Clone에 사용될 디버프 변수
    
    int debuffQueueLength { get { return GameManager.TrollerData.debuffQueueLength; } set { GameManager.TrollerData.debuffQueueLength = value; } } // 디버프 큐 길이
    //int debuffCount { get { return GameManager.TrollerData.debuffCount; } }
    
    Queue<IDebuff> debuffQueue { get { return GameManager.TrollerData.debuffQueue; } set { GameManager.TrollerData.debuffQueue = value; } } // 디버프 큐

    public PhysicsMaterial2D[] debuff_PhysicsMaterials { get { return GameManager.TrollerData.debuff_PhysicsMaterials; } } // 디버프 물리머테리얼 배열
        
    //[SerializeField] public TMP_Text[] TrapListTexts;
    TrapListUI trapListUI;

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

    public void SetTrap(Debuff debuff,Platform platform)
    {

        Collider2D platformCollider2D = platform.GetComponent<Collider2D>();
        SurfaceEffector2D surfaceEffector2D = platform.GetComponent<SurfaceEffector2D>();

        // 각 state에 따른  AddComponent 또는 Player 수치 변화 함수 호출 
        // 아래 소스를 대리자를 사용, platform 내에서 구현되게 로직 수정
        switch (debuff.state)
        {
            case Debuff_State.NoCollider: platformCollider2D.isTrigger = true; break; // 1번(충돌체 없애기)일 경우
            case Debuff_State.Surface: surfaceEffector2D.enabled = true; break; // 2번(표면이펙트)일 경우
            case Debuff_State.Spring: platformCollider2D.sharedMaterial = debuff_PhysicsMaterials[(int)Debuff_State.Spring]; break;
            case Debuff_State.Ice: platformCollider2D.sharedMaterial = debuff_PhysicsMaterials[(int)Debuff_State.Ice]; break;
            case Debuff_State.None:
                {
                    platformCollider2D.sharedMaterial = null;
                    platformCollider2D.isTrigger = false;
                    surfaceEffector2D.enabled = false;
                    break;
                }
            default: Debug.Log($"{debuff.state}는 현재 구현예정"); break;
                // 3번(플레이어 속도 순식간에 0으로 만들기)인 경우
        }
    }

    public void DisableTrap(Debuff debuff)
    {
        debuff.state = Debuff_State.None;
    }
}
