using TMPro;
using UnityEngine;

public class ScoreCountView : MonoBehaviour
{
	[SerializeField] private Transform player; // 플래이어 위치
	[SerializeField] private Transform startPoint; // 시작 지점
	[SerializeField] private Transform endPoint; // 마지막 지점

	private TMP_Text scoreText;

	private void Awake()
	{
		scoreText = GetComponent<TMP_Text>();
	}

	void Update()
	{
		// 시작 지점과 마지막 지점 사이의 y 거리 계산
		float totalYDistance = Mathf.Abs(endPoint.position.y - startPoint.position.y);

		// 시작 지점과 플레이어 사이의 y 거리 계산
		float playerYDistance = Mathf.Abs(player.position.y - startPoint.position.y);

		// 시작 지점부터 플레이어까지의 y 거리 백분율 계산
		float percentage = Mathf.Clamp((playerYDistance / totalYDistance) * 100f, 0f, 100f);

		// 백분율 값을 정수로 변환하여 표시
		int score = Mathf.RoundToInt(percentage);
		scoreText.text = "SCORE : " + score.ToString() + " %";
	}
}