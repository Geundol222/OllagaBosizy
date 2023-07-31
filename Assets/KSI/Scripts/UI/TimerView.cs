using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviour
{
	[SerializeField] private float limitTime = 300f; // 제한 시간 5분
	private float remainLimitTime; // 남은 제한 시간

	private TMP_Text timerText;
	
	private void Awake()
	{
		timerText = GetComponent<TMP_Text>();
	}

	private void Start()
	{
		DisplayTimer(limitTime);
	}

	private void DisplayTimer(float second)
	{ 
		remainLimitTime = second;
		StartCoroutine(UpdateTimerRoutine());
	}

	// 타이머 코루틴
	private IEnumerator UpdateTimerRoutine()
	{
		while (remainLimitTime >= 0)
		{
			int minutes = Mathf.FloorToInt(remainLimitTime / 60);
			int seconds = Mathf.FloorToInt(remainLimitTime % 60);
			timerText.text = $"{minutes:00} : {seconds:00}";

			remainLimitTime--;
			yield return new WaitForSeconds(1f);
		}
		TimeOver();
	}

	private void TimeOver()
	{
		Debug.Log("TimeOver");
		// TODO : 타임오버 UI 추가???
	}	
}