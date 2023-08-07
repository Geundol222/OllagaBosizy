using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrollerDataManager : MonoBehaviour
{
    public TrollerPlayerController trollerPlayerController;
    public Platform currentPlatform;
    public Platform prevPlatform;

    public int debuffQueueLength = 4;
    public int debuffCount = 4;

    public Debuff Original_Debuff;
    public Queue<IDebuff> debuffQueue;

    private void Awake()
    {
        Original_Debuff = new Debuff(0);
        debuffQueue = new Queue<IDebuff>();
    }
}
