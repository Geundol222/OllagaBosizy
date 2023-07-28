using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	[Header("Gizmo")]
	[SerializeField] bool debug;

	[Header("")]
	[SerializeField] private float maxSpeed;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float jumpPower;

	[Header("LayerMask")]
	[SerializeField] private LayerMask platformLayer;

	[Header("GFX")]
	[SerializeField] Transform gfx;

	private new Rigidbody2D rigidbody;
	private Animator animator;
	private Vector2 inputDirection = Vector2.zero;
	private bool isGround;
	

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		Move();
	}

	private void FixedUpdate()
	{
		GroundCheck();
	}

	public void Move()
	{
		// 최고 속력일 경우 힘을 가해도 속력이 빨라지지 않음
		if (inputDirection.x < 0 && rigidbody.velocity.x > -maxSpeed)
		{
			gfx.rotation = Quaternion.Euler(0, -90, 0);
			rigidbody.AddForce(Vector2.right * inputDirection.x * moveSpeed, ForceMode2D.Force);
		}
		else if (inputDirection.x > 0 && rigidbody.velocity.x < maxSpeed)
		{
			gfx.rotation = Quaternion.Euler(0, 90, 0);
			rigidbody.AddForce(Vector2.right * inputDirection.x * moveSpeed, ForceMode2D.Force);
		}
	}

	public void Jump()
	{
		rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
	}

	private void OnMove(InputValue value)
	{
		inputDirection = value.Get<Vector2>();
		animator.SetFloat("MoveSpeed", Mathf.Abs(inputDirection.x));

	}

	private void OnJump(InputValue value)
	{
		if (value.isPressed && isGround)
		Jump();
	}

	private void GroundCheck()
	{
		Debug.DrawRay(transform.position, Vector2.down * 0.5f, Color.red);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, platformLayer);
		if (hit.collider != null)
		{
			isGround = true;
			animator.SetBool("IsGround", true);
		}
		else
		{
			isGround = false;
			animator.SetBool("IsGround", false);
		}
	}
}