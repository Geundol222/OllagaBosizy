using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class CharacterNicknameDisplay : MonoBehaviourPun
{
	private TextMeshPro nicknameText;
	private LoginCanvas loginCanvas;

	private void Start()
	{
		DisplayNickname();
	}

	private void DisplayNickname()
	{
		if (loginCanvas != null)
		{
			string readnick = loginCanvas.reader["NICKNAME"].ToString();
			nicknameText.text = readnick;
			Debug.Log("Nickname: " + readnick);
		}
	}
}
