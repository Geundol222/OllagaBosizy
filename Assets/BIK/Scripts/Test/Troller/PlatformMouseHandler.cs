using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformMouseHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    private Platform platform;                                      // isClickable 함수 사용을 위한 Platform 참조
    private List<Platform> setTrapPlatforms { get { return GameManager.TrollerData._setTrapPlatforms; } }
    private int setTrapPlatformsCount { get { return GameManager.TrollerData._setTrapPlatforms.Count; } }

    private bool canMouseAction
    {
        get
        {
            if (setTrapPlatformsCount < GameManager.TrollerData.maxSetTrapPlatforms && GameManager.TrollerData.canSetTrap)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void Awake()
    {
        platform = GetComponent<Platform>();
    }

    private void Start()
    {
        StartCoroutine(TrollerControllerFindRoutine());
    }


    IEnumerator TrollerControllerFindRoutine()
    {
        yield return new WaitUntil(() => { return GameObject.Find("TrollerController(Clone)"); });
        if (GameManager.Team.GetTeam() == PlayerTeam.Troller)
            GameManager.TrollerData.trollerPlayerController = GameObject.Find("TrollerController(Clone)").GetComponent<TrollerPlayerController>();
        else
            yield break;
        yield break;
    }

    /// <summary>
    /// 클릭 된다면 clickAbleState 가 true면 ..
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Team.GetTeam() != PlayerTeam.Troller)
            return;

        if (!canMouseAction)
        {
            // 클릭 불가 효과음 추가 23.08.14
            GameManager.Sound.PlaySound("Stage/Blocked", Audio.UISFX);
            return;
        }
        // 마우스 오버 효과음 추가 23.08.14
        GameManager.Sound.PlaySound("Stage/MouseOver", Audio.UISFX);
        if (platform.IsClickable)
        {
            platform.ShowSetTrapButton();
        }
    }

    /// <summary>
    /// 마우스가 바깥으로 나간다면 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.Team.GetTeam() != PlayerTeam.Troller)
            return;

        platform.SwitchRenderColorExit();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.Team.GetTeam() != PlayerTeam.Troller)
            return;

        if (!canMouseAction)
            return;
        platform.SwitchRenderColorEnter();
    }

}
