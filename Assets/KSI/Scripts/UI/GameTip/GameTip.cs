using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameTip : MonoBehaviour
{
	public GameTipData[] gameTipdatas;
	public TextMeshProUGUI text;

	private int randomIndex;
	private string gameTipText;

	private void Awake()
	{
		StartCoroutine(DisplayRandomGameTipRoutine());
	}

	private IEnumerator DisplayRandomGameTipRoutine()
	{
		while (true)
		{
			if (text != null && gameTipdatas.Length > 0)
			{
				randomIndex = Random.Range(0, gameTipdatas.Length);
				gameTipText = "Game Tip : " + gameTipdatas[randomIndex].gameTipDescription;
				text.text = gameTipText;
				Debug.Log(gameTipText);
			}
			else
			{
				Debug.Log("GameTipData 할당되지 않음");
			}

			yield return new WaitForSeconds(1f);
		}		
	}
}
