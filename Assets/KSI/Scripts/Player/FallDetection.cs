using UnityEngine;

public class FallDetection : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Animator animator;

    private int fallPoint; // 떨어진 지점
    [SerializeField] private int fallDegree = 10; // 떨어진 정도

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
			animator.SetBool("IsFalling", true);
		}
		else
		{
			Debug.Log("플레이어가 일어섰습니다.!");
			animator.SetBool("IsFalling", false);
		}
	}
}
