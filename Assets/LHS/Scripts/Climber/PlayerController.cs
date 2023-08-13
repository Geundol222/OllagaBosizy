using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviourPun
{
	[Header("Gizmo")]
	[SerializeField] bool debug;

	[Header("LayerMask")]
	[SerializeField] private LayerMask platformLayer;

    [Header("Collider")]
	[SerializeField] Collider2D platformTrigger;

	[Header("Other")]
    [SerializeField] TMP_Text nickNameText;

    private GameObject debuffList;
    private PlayerInput inputAction;
    private CinemachineVirtualCamera playerCamera;
	private Vector3 prevPlayerPosition;
	private Vector3 curPlayerPosition;
	private Animator animator;
	private bool isGround;

    public bool IsGround { get { return isGround; } }

    private void Awake()
	{
        if (GameManager.Team.GetTeam() != PlayerTeam.Climber)
            Destroy(gameObject.GetComponent<PlayerController>());

        prevPlayerPosition = transform.position;
		
		animator = GetComponent<Animator>();
        inputAction = GetComponent<PlayerInput>();
        playerCamera = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
		debuffList = GameObject.Find("TrapList");

        if (!photonView.IsMine)
            Destroy(inputAction);

        if (photonView.IsMine)
        {
			playerCamera.Follow = transform;
			playerCamera.LookAt = transform;
        }
    }

	private void Start()
	{
		StartCoroutine(NetworkConnectChecker());

		if (GameManager.Team.GetTeam() == PlayerTeam.Climber)
		{
			if (photonView.IsMine)
				nickNameText.text = "You";

            Destroy(debuffList);
		}
    }

	IEnumerator NetworkConnectChecker()
	{
		yield return new WaitUntil(() => { return PhotonNetwork.IsConnected; });

		platformTrigger.enabled = true;

		if (PhotonNetwork.IsConnected)
			yield break;
	}

	private void FixedUpdate()
	{
        if (GameManager.Team.GetTeam() == PlayerTeam.Climber)
            GroundCheck();        
    }

	private void GroundCheck()
	{
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.2f, 0.2f), 0, Vector2.down, 0.5f, platformLayer);
		if (hit.collider != null)
		{
			isGround = true;
            animator.SetBool("IsGround", true);
            curPlayerPosition = transform.position;
			HowmuchFallingHeight();
        }
		else
		{
			isGround = false;
			animator.SetBool("IsGround", false);
            animator.SetBool("IsFall", false);
        }
	}

	private void HowmuchFallingHeight()
	{
		if (isGround && prevPlayerPosition.y > curPlayerPosition.y)
		{
			if (curPlayerPosition.y > 0 && (prevPlayerPosition.y - curPlayerPosition.y) > 10)
			{
                animator.SetBool("IsFall", true);
            }
			else if (curPlayerPosition.y <= 0 && (prevPlayerPosition.y + Mathf.Abs(curPlayerPosition.y)) > 10)
			{
                animator.SetBool("IsFall", true);
            }
            else
			{
                animator.SetBool("IsFall", false);
            }
		}
		else
		{
            animator.SetBool("IsFall", false);
        }

        prevPlayerPosition = curPlayerPosition;
    }

	public void InputEnable()
	{
        if (!photonView.IsMine)
			return;

		inputAction.enabled = true;
	}

	public void InputDisable()
	{
        if (!photonView.IsMine)
            return;

        inputAction.enabled = false;
	}
}