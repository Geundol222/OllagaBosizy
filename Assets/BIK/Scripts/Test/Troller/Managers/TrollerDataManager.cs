using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrollerDataManager : MonoBehaviour
{
    public TrollerPlayerController trollerPlayerController;         // 방해자 컨트롤러
    public Platform currentPlatform;                                // 현재 선택된 플랫폼
    public Platform prevPlatform;                                   // 이전에 선택한 플랫폼
    public bool canSetTrap;                                         // 함정 설치여부에 따라 클릭 가능한지 확인하는 변수
    public bool FindComplete { get; private set; }

    public Debuff Original_Debuff;                                  // clone 시킬 Debuff 클래스
    public Queue<IDebuff> debuffQueue;                              // Debuff들을 담을 Queue
    public PhysicsMaterial2D[] debuff_PhysicsMaterials;             // Debuff 효과에 대한 물리머테리얼을 담은 배열

    public float debuffSetCoolTime = 2;                             // 함정 설치 쿨타임
    public int debuffQueueLength = 4;                               // Debuff를 담을 Queue의 최대크기
    public int debuffCount { get { return debuffQueue.Count; } }    // 현재 Queue의 크기

    private List<Platform> setTrapPlatforms;
    public List<Platform> _setTrapPlatforms { get { return setTrapPlatforms; } set { setTrapPlatforms = value; } }
    public int maxSetTrapPlatforms = 5;

    public LeftTrapUI leftTrapUI;
    public TrapListUI trapListUI;
    public DebuffManager debuffManager;

    private void Awake()
    {
        Original_Debuff = new Debuff(0);
        debuffQueue = new Queue<IDebuff>();
        debuff_PhysicsMaterials = new PhysicsMaterial2D[(int)Debuff_State.Length];
        canSetTrap = true;
        setTrapPlatforms = new List<Platform>();
        InitPhysicsList();
    }

    public void Init()
    {
        FindComplete = false;

        if (GameManager.Team.GetTeam() == PlayerTeam.Troller)
            StartCoroutine(FindTrapRoutine());

        debuffManager = GameObject.Find("DebuffManager").GetComponent<DebuffManager>();
    }

    IEnumerator FindTrapRoutine()
    {
        yield return new WaitUntil(() => { return GameObject.Find("TrollerController(Clone)"); });

        leftTrapUI = GameObject.Find("LeftTrap(Clone)").GetComponent<LeftTrapUI>();
        trapListUI = GameObject.Find("TrapList(Clone)").GetComponent<TrapListUI>();

        FindComplete = true;

        yield break;
    }

    public void InitPhysicsList()
    {
        debuff_PhysicsMaterials[(int)Debuff_State.Ice] = GameManager.Resource.Load<PhysicsMaterial2D>("Debuff/Ice");
        debuff_PhysicsMaterials[(int)Debuff_State.Spring] = GameManager.Resource.Load<PhysicsMaterial2D>("Debuff/Spring");
    }
}
