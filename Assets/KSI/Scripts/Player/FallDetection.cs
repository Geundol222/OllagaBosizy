using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetection : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Animator animator;

    private int fallPoint; // 떨어진 지점
    private int fallDegree = 5; // 떨어진 정도

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		fallPoint = (int)rigidbody.position.y; 
	}

	private void Update()
	{
		int currentPoint = (int)rigidbody.position.y;
		int heightDifference = fallPoint - currentPoint;

		if (currentPoint > fallPoint)
		{
			fallPoint = currentPoint;
		}

		if (heightDifference >= fallDegree) 
		{
			Debug.Log("플레이어가 떨어졌습니다!");

			// animator.SetBool("isFalling", true);
		}
		else
		{
			animator.SetBool("isFalling", false);
		}
	}
}
