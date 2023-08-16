using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//여러가지 유저에게 알려줄 것이 있다면 알려주는 image
public class LogImage : MonoBehaviour
{
    private Animator anim;
    public TMP_Text text;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.SetTrigger("IsOpen");
    }

    public void SetText(string str)
    {
        text.text = str;
    }

    public void Close()
    {
        StartCoroutine(CloseRoutine());
    }

    IEnumerator CloseRoutine()
    {
        anim.SetTrigger("IsClose");
        yield return new WaitForSeconds(0.5f);
        text.text = "";
        gameObject.SetActive(false);
        yield return null;
    }
}
