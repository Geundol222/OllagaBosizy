using System.Collections.Generic;
using UnityEngine;

public class FallDetection : MonoBehaviour
{
    [SerializeField] private List<float> fallHeights; // 떨어질 지점들을 List로 관리

	private Animator animator;
	//private bool isFalling = false;

	private void Start()
	{
		animator = GetComponent<Animator>();
		//isFalling = false;
	}

	private void Update()
	{
	// 현재 위치의 높이를 가져옴
	float currentHeight = transform.position.y;

	// 떨어지는 애니메이션이 재생 중이 아니라면
	
		// 떨어지는 지점들을 순차적으로 체크하여 애니메이션 재생
		foreach (float fallHeight in fallHeights)
		{
			if (currentHeight >= fallHeight)
			{
				animator.SetTrigger("IsFall"); // 애니메이션 트리거 "Fall"을 호출하여 떨어지는 애니메이션 재생
				break;
			}
		}
	}
	
}