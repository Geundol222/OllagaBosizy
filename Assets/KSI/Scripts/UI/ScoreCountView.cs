using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class ScoreCountView : MonoBehaviour
{
	[SerializeField] private DataManager dataManager;

	private TMP_Text text;

	private void Awake()
	{
		text = GetComponent<TMP_Text>();
	}

	private void OnEnable()
	{
		dataManager.OnCurrentScoreChanged += ChangeScore;
	}

	private void OnDisable()
	{
		dataManager.OnCurrentScoreChanged -= ChangeScore;
	}

	private void ChangeScore(int score)
    {
		text.text = score.ToString();
	}   
}
