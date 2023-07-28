using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformMouseHandler : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Color pointerOverColor;                // 마우스 포인터 올려졌을 때 바꿔줄 색
    private Color pointerOutColor = Color.white;                    // 마우스 포인터 빠져나왔을 때 바꿔줄 색(무색)
    private Renderer[] renderer;                                    // 자식개체 렌더러를 담을 배열
    private Platform platform;                                      // isClickable 함수 사용을 위한 Platform 참조

    private void Awake()
    {
        // 자식 개체의 Renderer 컴포넌트 담기
        renderer = GetComponentsInChildren<Renderer>();
        platform = GetComponent<Platform>();
    }

    public void SetRenderer(bool state)
    {
        // 만약 바뀐 State가 false면 렌더러 컬러를 없애줌(
        if (state)
        {
            SwitchRenderColor(pointerOverColor);
        }
        else
        {
            SwitchRenderColor(pointerOutColor);
        }
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
        SwitchRenderColor(pointerOutColor);
        //Debug.Log($"Exit from {gameObject.name} !!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (platform.IsClickable)
        {
            SwitchRenderColor(pointerOverColor);
            //Debug.Log($"Enter the {gameObject.name} !!");
        }
    }

    public void SwitchRenderColor(Color color)
    {
        foreach (Renderer renderer in renderer)
        {
            if (renderer != null)
                renderer.material.color = color;
        }
    }
}
