using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] Slider slider;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        anim.SetBool("IsActive", true);
    }

    public void FadeOut()
    {
        anim.SetBool("IsActive", false);
    }

    public void SetProgress(float progress)
    {
        slider.value = progress;
    }
}