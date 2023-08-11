using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrollerCameraController : MonoBehaviourPunCallbacks
{
    [SerializeField] Texture2D cursor;
    [SerializeField] float cameraMoveSpeed;
    [SerializeField] float padding;
    [SerializeField] CinemachineVirtualCamera vcam;

    Vector3 cameraMoveDir;
    bool pressArrows = false;
    bool foundCamera = false;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;                         // 마우스 커서 게임 윈도우 안에서만 
        //Cursor.SetCursor(cursor, Vector3.zero, CursorMode.ForceSoftware);   // 마우스 커서 이미지 넣기
        StartCoroutine(FindCameraCoroutine());
    }

    IEnumerator FindCameraCoroutine()
    {
        yield return new WaitUntil(() => { return vcam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>(); });
        foundCamera = true;
        vcam.transform.position = new Vector3(transform.position.x, transform.position.y, vcam.transform.position.z);
        yield break;
    }


    public override void OnDisable()
    {
        base.OnDisable();
        Cursor.lockState = CursorLockMode.None;
        StopAllCoroutines();
    }

    private void LateUpdate()
    {
        if (foundCamera && photonView.IsMine)
            CameraMove();
    }

    private void FixedUpdate()
    {
        if (foundCamera && photonView.IsMine)
            CheckCameraInCamZone();
    }

    /// <summary>
    /// 시네머신 카메라가 CamZone 영역 내에 있는지 확인해주는 함수
    /// 영역 밖으로 못나가게 영역 밖의 위치에서는 이동 값을 0으로
    /// </summary>
    private void CheckCameraInCamZone()
    {
        if (vcam.transform.position.x <= -16.5 || vcam.transform.position.x >= 16)
        {
            cameraMoveDir.x = 0;
        }

        if (vcam.transform.position.y <= -0.5 || vcam.transform.position.y >= 168)
        {
            cameraMoveDir.y = 0;
        }
    }

    private void CameraMove()
    {
        vcam.transform.Translate(Vector3.right * cameraMoveDir.x * cameraMoveSpeed * Time.deltaTime, Space.World);
        vcam.transform.Translate(Vector3.up * cameraMoveDir.y * cameraMoveSpeed * Time.deltaTime, Space.World);
    }

    private void OnMove(InputValue value) // 방향키 입력 감지
    {
        // 방향키 입력을 확인하여 pressArrows의 값을 유동적으로 관리
        // OnPointer쪽에서 방향키 입력 중인 경우엔 cameraMoveDir 을 0으로 만들지 못하게 처리
        Vector2 keyboardPos = value.Get<Vector2>();
        if (keyboardPos.x != 0 || keyboardPos.y != 0) // 방향키 입력 감지
        {
            pressArrows = true;
        }
        else
        {
            pressArrows = false;
        }

        cameraMoveDir = keyboardPos;
    }

    private void OnPointer(InputValue value) // 마우스 커서 이동감지
    {
        // 만약 현재 방향키로 카메라 전환이 이뤄지고 있다면, 마우스를 이용한 카메라 이동이 불가능하게 처리
        if (pressArrows)
            return;

        Vector2 mousePos = value.Get<Vector2>();

        if (-10 < mousePos.x && mousePos.x <= 0 + padding)
        {
            cameraMoveDir.x = -1;
        }
        else if (mousePos.x >= Screen.width - padding && mousePos.x <= Screen.width + 10)
        {
            cameraMoveDir.x = 1;
        }
        else
        {
            cameraMoveDir.x = 0;
        }

        if (-10 < mousePos.y && mousePos.y <= 0 + padding) // 마우스의 y 위치 값이 패딩 값보다 같거나 작다.
        {
            cameraMoveDir.y = -1;
        }
        else if (mousePos.y >= Screen.height - padding && mousePos.y <= Screen.height + 10) // 마우스의 y 위치 값이 최상단의 패딩 값에 해당하는 구간이다.
        {
            cameraMoveDir.y = 1;
        }
        else
        {
            cameraMoveDir.y = 0;
        }
    }


}
