using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviourPun
{
    [Header("Value")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;

    [Header("GFX")]
    [SerializeField] Transform gfx;

    private PlayerController controller;
    private Animator animator;
    private Rigidbody2D rigid;
    private Vector2 inputDirection;
    private string climberType;             // 점프(boy girl)에 대한 효과음 로드를 위하여 .. 
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        InitClimberType();
    }

    private void InitClimberType()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            climberType = PhotonNetwork.LocalPlayer.GetClimber().ToString();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Team.GetTeam() == PlayerTeam.Climber)
            Move();
    }

    public void Move()
    {
        // 최고 속력일 경우 힘을 가해도 속력이 빨라지지 않음
        if (inputDirection.x < 0 && rigid.velocity.x > -maxSpeed)
        {
            gfx.rotation = Quaternion.Euler(0, -90, 0);
            rigid.AddForce(Vector2.right * inputDirection.x * moveSpeed * Time.deltaTime, ForceMode2D.Force);
        }
        else if (inputDirection.x > 0 && rigid.velocity.x < maxSpeed)
        {
            gfx.rotation = Quaternion.Euler(0, 90, 0);
            rigid.AddForce(Vector2.right * inputDirection.x * moveSpeed * Time.deltaTime, ForceMode2D.Force);
        }
    }

    public void Jump()
    {
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private void OnMove(InputValue value)
    {
        inputDirection = value.Get<Vector2>();
        animator.SetFloat("MoveSpeed", Mathf.Abs(inputDirection.x));
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed && controller.IsGround)
        {
            PlayJumpSound();
            animator.SetTrigger("Jump");
            Jump();
        }
    }

    public void PlayJumpSound()
    {
        if (climberType == "")
        {
            InitClimberType();
        }
        string path = $"Player/Jump/{climberType}-Jump-{Random.Range(1, 3)}";
        photonView.RPC("PlaySound", RpcTarget.AllBufferedViaServer, path);
    }
    
    [PunRPC]
    public void PlaySound(string path)
    {
        GameManager.Sound.PlaySound(path, Audio.SFX,gameObject.transform.position);
    }

    public void PlayScreamSoundEnd()
    {
        // if (!photonView.IsMine)
        //     return;

        Debug.Log("비명 발생");

        if (climberType == "")
        {
            InitClimberType();
        }
        string path = $"Player/Fall/{climberType}-Fall-3";
        photonView.RPC("PlaySound", RpcTarget.AllBufferedViaServer, path);
    }

    public void PlayRunningSound()
    {
        // if (!photonView.IsMine)
        //     return;

        Debug.Log("발소리 발생");

        string path = $"Player/Run/run_{Random.Range(1, 5)}";
        photonView.RPC("PlaySound", RpcTarget.AllBufferedViaServer, path);
    }
}
