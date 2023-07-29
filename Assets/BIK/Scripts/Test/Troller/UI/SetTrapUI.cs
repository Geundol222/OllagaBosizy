using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetTrapUI : InGameUI, IPointerClickHandler
{
    Platform platform;
    Animator animator;
    Coroutine buttonCloseCoroutine;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void SetParentPlatform(Platform platform)
    {
        this.platform = platform;
    }
     

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("함정설치");
    }

    public void OnClickCloseArea()
    {
        if (platform == null)
            return;

        buttonCloseCoroutine = StartCoroutine(HideSetTrapButton());
    }

    IEnumerator HideSetTrapButton()
    {
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.3f);
        platform.HideSetTrapButton();
    }

    private void OnDisable()
    {
        StopCoroutine(buttonCloseCoroutine);
    }
}
