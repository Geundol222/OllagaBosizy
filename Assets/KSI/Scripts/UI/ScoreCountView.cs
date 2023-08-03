using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCountView : MonoBehaviour
{
	[Header("Transform")]
	[SerializeField] private Transform player; // 플래이어 위치
	[SerializeField] private Transform startPoint; // 시작 지점
	[SerializeField] private Transform endPoint; // 마지막 지점

	[Header("ScoreUI")]
	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private Slider scoreSlider;

	private void Awake()
	{
		scoreText = GetComponentInChildren<TextMeshProUGUI>();
	}

	private void Start()
	{
		scoreSlider.maxValue = 100;
		scoreSlider.minValue = 0;

		scoreSlider.value = 0;
	}

	private void Update()
	{
		// 시작 지점과 마지막 지점 사이의 y 거리 계산
		float totalYDistance = Mathf.Abs(endPoint.position.y - startPoint.position.y);

		// 시작 지점과 플레이어 사이의 y 거리 계산
		float playerYDistance = Mathf.Abs(player.position.y - startPoint.position.y);

		// 시작 지점부터 플레이어까지의 y 거리 백분율 계산
		float percentage = Mathf.Clamp((playerYDistance / totalYDistance) * 100f, 0f, 100f);

		// 백분율 값을 정수로 변환하여 텍스트로 표시
		int score = Mathf.RoundToInt(percentage);
		scoreText.text = score.ToString() + "%";
		// 백분율 값을 슬라이더에 연결
		scoreSlider.value = score;
	}
}