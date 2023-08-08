using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformMouseHandler : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{

    private Platform platform;                                      // isClickable 함수 사용을 위한 Platform 참조
    private List<Platform> setTrapPlatforms { get { return GameManager.TrollerData.setTrapPlatforms; }  }
    private bool canMouseAction { get { 
            if(setTrapPlatforms.Count < GameManager.TrollerData.maxSetTrapPlatforms && GameManager.TrollerData.canSetTrap)
            {
                return true;
            } else
            {
                return false;
            }
        } }

    private void Awake()
    {
        platform = GetComponent<Platform>();
    }

    private void SetTrollerController()
    {
        if (GameManager.TrollerData.trollerPlayerController == null)
        {
            if(GameObject.Find("TrollerController(Clone)").GetComponent<TrollerPlayerController>())
                GameManager.TrollerData.trollerPlayerController = GameObject.Find("TrollerController(Clone)").GetComponent<TrollerPlayerController>();
        }
    }

    /// <summary>
    /// 클릭 된다면 clickAbleState 가 true면 ..
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canMouseAction)
            return;
        if (platform.IsClickable)
        {
            SetTrollerController();
            platform.ShowSetTrapButton();
        }                
    }
      
    /// <summary>
    /// 마우스가 바깥으로 나간다면 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        SetTrollerController();
        platform.SwitchRenderColorExit();
    }
      
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!canMouseAction)
            return;
        SetTrollerController();
        platform.SwitchRenderColorEnter();
    }
      
}
