using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameTip : MonoBehaviour
{
    [SerializeField] private GameTipData gameTipData;
    public GameTipData GameTipData { set { gameTipData = value; } }

    public void WatchGameTipInfo()
    {
        Debug.Log("GameTip Number : " + gameTipData.GameTipNumber);
        Debug.Log("GameTip Description : " + gameTipData.GameTipDescription);
    }
}
