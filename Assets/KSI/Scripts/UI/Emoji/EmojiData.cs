using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmojiData", menuName = "Data/Emoji")]
public class EmojiData : ScriptableObject
{
	public int emojiNumber;
	public string emojiDescription;
}
