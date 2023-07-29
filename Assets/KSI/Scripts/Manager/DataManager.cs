using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
	// �ְ� ����
    [SerializeField] private int bestScore;
	public int BestScore
	{
		get { return bestScore; }
		set
		{
			if (bestScore != value)
				OnBestScoreChanged?.Invoke(value);
			bestScore = value;
		}
	}
	public event UnityAction<int> OnBestScoreChanged;

	// ���� ����
	[SerializeField] private int currentScore;
	public int CurrentScore
	{
		get { return currentScore; }
		set
		{
			OnCurrentScoreChanged?.Invoke(value);	
			currentScore = value;

			if (currentScore > BestScore)
				BestScore = currentScore;
		}
	}
	public event UnityAction<int> OnCurrentScoreChanged;
}
