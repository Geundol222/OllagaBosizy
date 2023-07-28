using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformMouseHandler : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{

    private Platform platform;                                      // isClickable 함수 사용을 위한 Platform 참조
                                
    private void Awake()
    {
        // 자식 개체의 Renderer 컴포넌트 담기
        platform = GetComponent<Platform>();
    }
      
    /// <summary>
    /// 클릭 된다면 clickAbleState 가 true면 ..
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (platform.IsClickable)
        {
            Debug.Log($"Clicked {gameObject.name} !!");
        }                
    }
      
    /// <summary>
    /// 마우스가 바깥으로 나간다면 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        platform.SwitchRenderColorExit();
    }
      
    public void OnPointerEnter(PointerEventData eventData)
    {
        platform.SwitchRenderColorEnter();
    }
      
}
