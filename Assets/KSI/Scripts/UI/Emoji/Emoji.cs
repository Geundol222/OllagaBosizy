using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Emoji : MonoBehaviour
{
	public EmojiData[] emojidatas;
	public TextMeshProUGUI text;

	private int randomIndex;
	private string emojiText;

	private void OnEnable()
	{
		StartCoroutine(DisplayRandomEmojiRoutine());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator DisplayRandomEmojiRoutine()
	{
		while (true)
		{
			if (text != null && emojidatas.Length > 0)
			{
				randomIndex = Random.Range(0, emojidatas.Length);
				emojiText = emojidatas[randomIndex].emojiDescription;
				text.text = emojiText;
				Debug.Log(emojiText);
			}
			else
			{
				Debug.Log("EmojiData 할당되지 않음");
			}

			yield return new WaitForSeconds(1f);
		}
	}
}
