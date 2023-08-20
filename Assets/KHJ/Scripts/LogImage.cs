using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//여러가지 유저에게 알려줄 것이 있다면 알려주는 image
public class LogImage : MonoBehaviour
{
    private Animator anim;          //Logimage의 애니메이터
    public TMP_Text text;           //유저에게 알려줄 것들을 적은 text
    LobbyManager manager;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        manager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
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
        manager.PlayUIButtonClickSound();
        anim.SetTrigger("IsClose");
        yield return new WaitForSeconds(0.5f);
        text.text = "";
        gameObject.SetActive(false);
        yield return null;
    }
}
