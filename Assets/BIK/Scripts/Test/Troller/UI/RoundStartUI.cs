using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class RoundStartUI : GameSceneUI
{
    Animator animator;
    [SerializeField] TMP_Text subtitle;
    [SerializeField] Camera stackCamera;
    [SerializeField] Image letterBox;
    [SerializeField] GameObject climberAvatar;
    [SerializeField] GameObject trollerAvatar;
    [SerializeField] Canvas canvasOfLetterBox;
    string[] subtitles;

    protected override void Awake()
    {
        base.Awake();
        if (!photonView.IsMine)
        {
            return;
        }
        // 캔버스 렌더 카메라 설정
        canvasOfLetterBox.worldCamera = Camera.main;
        // 애니메이터
        animator = GetComponent<Animator>();
        // 텍스트
        subtitles = new string[] { "발판에 함정을 설치해 등반을 방해하세요!","어떻게든 높이 올라가세요!"};
        // 메인 카메라에 Stack overlay카메라 추가
        var cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(stackCamera);
    }

    private void Start()
    {
        RoundStart();
    }

    public void Init()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        canvasOfLetterBox.renderMode = RenderMode.WorldSpace;
        canvasOfLetterBox.transform.position = new Vector3(canvasOfLetterBox.transform.position.x, canvasOfLetterBox.transform.position.y, 0);
        climberAvatar.SetActive(false);
        trollerAvatar.SetActive(false);
        subtitle.text = "";
        if (GameManager.Team.GetTeam() == PlayerTeam.Troller)
        {
            trollerAvatar.SetActive(true);
            subtitle.text = subtitles[0];

        }
        else if (GameManager.Team.GetTeam() == PlayerTeam.Climber)
        {
            climberAvatar.SetActive(true);
            subtitle.text = subtitles[1];
        }
        else
        {
            Debug.Log("오류");
        }

        stackCamera.transform.position = new Vector3(0, stackCamera.transform.position.y, stackCamera.transform.position.z);
        letterBox.rectTransform.offsetMin = new Vector2( -1920, letterBox.rectTransform.offsetMin.y);
       
    }

    public void RoundStart()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        Init();
        animator.SetTrigger("Start");
    }
}
