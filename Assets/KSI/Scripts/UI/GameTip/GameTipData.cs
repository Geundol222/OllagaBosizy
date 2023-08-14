using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "GameTipData", menuName = "Data/GameTip")]
public class GameTipData : ScriptableObject
{
	public int gameTipNumber;
	public string gameTipDescription;
}
