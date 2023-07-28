using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class TrollerController : MonoBehaviourPunCallbacks
{
    [SerializeField] Texture2D cursor;
    [SerializeField] float cameraMoveSpeed;
    [SerializeField] float padding;
    Vector3 cameraMoveDir;
    bool pressArrows = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;                         // 마우스 커서 게임 윈도우 안에서만 
        Cursor.SetCursor(cursor, Vector3.zero, CursorMode.ForceSoftware);   // 마우스 커서 이미지 넣기
    }

    private void OnDisable()
    {
        Cursor.lockState -= CursorLockMode.None;
    }

    private void LateUpdate()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        transform.Translate(Vector3.right * cameraMoveDir.x * cameraMoveSpeed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.up * cameraMoveDir.y * cameraMoveSpeed * Time.deltaTime, Space.World);
    
    }

    private void OnMove(InputValue value) // 방향키 입력 감지
    {
        // 방향키 입력을 확인하여 pressArrows의 값을 유동적으로 관리
        // OnPointer쪽에서 방향키 입력 중인 경우엔 cameraMoveDir 을 0으로 만들지 못하게 처리
        Vector2 keyboardPos = value.Get<Vector2>();
        Debug.Log($"x {keyboardPos.x} y {keyboardPos.y}");
        if (keyboardPos.x != 0 || keyboardPos.y != 0) // 방향키 입력 감지
        {
            pressArrows = true;
        } else
        {
            pressArrows = false;
        }

        if(keyboardPos.x < 0 && transform.position.x > -16)
        {
            //좌측으로 카메라 position 이동
            Debug.Log($"카메라 이동 현재 x 값 : {transform.position.x}");
            cameraMoveDir.x = -1;
        } else if (keyboardPos.x > 0 && transform.position.x < 16)
        {
            //우측으로 카메라 position 이동
            cameraMoveDir.x = 1;
        } else
        {
            cameraMoveDir.x = 0;
        }

        if(keyboardPos.y < 0 && transform.position.y > -0.5)
        {
            // y가 -1
            //하단으로 카메라 이동
            cameraMoveDir.y = -1;
        } else if(keyboardPos.y > 0 && transform.position.y < 33)
        {
            // y가 1
            //상단으로 카메라 이동
            cameraMoveDir.y = 1;
        } else
        {
            cameraMoveDir.y = 0;
        }

    }

    private void OnPointer(InputValue value) // 마우스 커서 이동감지
    {
        // 만약 현재 방향키로 카메라 전환이 이뤄지고 있다면, 마우스를 이용한 카메라 이동이 불가능하게 처리
        if (pressArrows)
            return;

        Vector2 mousePos = value.Get<Vector2>();
        Debug.Log($"x {mousePos.x}  y {mousePos.y} ");

        if( -10 < mousePos.x && mousePos.x <= 0 +padding && transform.position.x > -16)
        {
            cameraMoveDir.x = -1;
        } else if(mousePos.x >= Screen.width - padding && mousePos.x <= Screen.width + 10 && transform.position.x < 16)
        {
            cameraMoveDir.x = 1;
        }
        else
        {
            cameraMoveDir.x = 0;
        }

        if( -10 < mousePos.y && mousePos.y <= 0 + padding && transform.position.y > -0.5 ) // 마우스의 y 위치 값이 패딩 값보다 같거나 작다.
        {
            cameraMoveDir.y = -1;
        } else if(mousePos.y >= Screen.height - padding && mousePos.y <= Screen.height + 10 && transform.position.y < 33) // 마우스의 y 위치 값이 최상단의 패딩 값에 해당하는 구간이다.
        {
            cameraMoveDir.y = 1;
        } else
        {
            cameraMoveDir.y = 0;
        }    
    }


}
