using Cinemachine;
using Photon.Pun;
using System.Collections;
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

    private GameObject debuffList;
    private PlayerInput inputAction;
    private CinemachineVirtualCamera playerCamera;
    private CinemachineVirtualCamera trollerCamera;
	private Vector3 prevPlayerPosition;
	private Vector3 curPlayerPosition;
	private Animator animator;
	private bool isGround;

    public bool IsGround { get { return isGround; } }

    private void Awake()
	{
		prevPlayerPosition = transform.position;
		
		animator = GetComponent<Animator>();
        inputAction = GetComponent<PlayerInput>();
        playerCamera = GameObject.Find("Climber_Cam").GetComponent<CinemachineVirtualCamera>();
        trollerCamera = GameObject.Find("Troller_Cam").GetComponent<CinemachineVirtualCamera>();

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

        Destroy(debuffList);
        Destroy(trollerCamera.gameObject);
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
        GroundCheck();        
    }

	private void GroundCheck()
	{
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.2f, 0.2f), 0, Vector2.down, 0.5f, platformLayer);
		DrawBox(transform.position, transform.rotation, new Vector2(0.2f, 0.2f), Color.red);
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
			if (curPlayerPosition.y > 0 && (prevPlayerPosition.y - curPlayerPosition.y) > 5)
			{
                animator.SetBool("IsFall", true);
            }
			else if (curPlayerPosition.y <= 0 && (prevPlayerPosition.y + Mathf.Abs(curPlayerPosition.y)) > 5)
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

    public void DrawBox(Vector2 pos, Quaternion rot, Vector2 scale, Color c)
    {
        // create matrix
        Matrix4x4 m = new Matrix4x4();
        m.SetTRS(pos, rot, scale);

        var point1 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, 0.5f));
        var point2 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, 0.5f));
        var point3 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, -0.5f));
        var point4 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, -0.5f));

        var point5 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, 0.5f));
        var point6 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, 0.5f));
        var point7 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, -0.5f));
        var point8 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, -0.5f));

        Debug.DrawLine(point1, point2, c);
        Debug.DrawLine(point2, point3, c);
        Debug.DrawLine(point3, point4, c);
        Debug.DrawLine(point4, point1, c);

        Debug.DrawLine(point5, point6, c);
        Debug.DrawLine(point6, point7, c);
        Debug.DrawLine(point7, point8, c);
        Debug.DrawLine(point8, point5, c);

        Debug.DrawLine(point1, point5, c);
        Debug.DrawLine(point2, point6, c);
        Debug.DrawLine(point3, point7, c);
        Debug.DrawLine(point4, point8, c);
    }
}