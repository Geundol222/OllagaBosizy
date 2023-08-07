using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrollerDataManager : MonoBehaviour
{
    public TrollerPlayerController trollerPlayerController;
    public Platform currentPlatform;
    public Platform prevPlatform;


    public Debuff Original_Debuff;
    public Queue<IDebuff> debuffQueue;
    public PhysicsMaterial2D[] debuff_PhysicsMaterials;

    public int debuffQueueLength = 4;
    public int debuffCount { get { return debuffQueue.Count; } }

    private void Awake()
    {
        Original_Debuff = new Debuff(0);
        debuffQueue = new Queue<IDebuff>();
        debuff_PhysicsMaterials = new PhysicsMaterial2D[(int)Debuff_State.Length];
        InitPhysicsList();
    }

    public void InitPhysicsList()
    {
        debuff_PhysicsMaterials[(int)Debuff_State.Ice] = GameManager.Resource.Load<PhysicsMaterial2D>("Debuff/Ice");
        debuff_PhysicsMaterials[(int)Debuff_State.Spring] = GameManager.Resource.Load<PhysicsMaterial2D>("Debuff/Spring");
    }
}
