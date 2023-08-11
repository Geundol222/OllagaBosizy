using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameTipData", menuName = "Data/GameTip")]
public class GameTipData : ScriptableObject
{
    [SerializeField] private string gameTipNumber;
    public string GameTipNumber { get { return gameTipNumber; } }

    [SerializeField] private string gameTipDescription;
    public string GameTipDescription { get { return gameTipDescription; } }
}