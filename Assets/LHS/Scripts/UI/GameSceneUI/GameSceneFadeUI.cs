using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneFadeUI : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeOut()
    {
        anim.Play("FadeOutGame");
    }

    public void TimeOut()
    {
        anim.Play("TimesUP");
    }
}
